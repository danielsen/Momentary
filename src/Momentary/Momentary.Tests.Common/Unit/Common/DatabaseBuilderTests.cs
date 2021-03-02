using System;
using Microsoft.Extensions.Configuration;
using Momentary.Common;
using Momentary.Tests.Common.Common.Data.Fakes;
using NSubstitute;
using NUnit.Framework;

namespace Momentary.Tests.Common.Unit.Common
{
    [TestFixture, NonParallelizable]
    public class DatabaseBuilderTests
    {
        private UnitTestDatabaseBuilder _builder;
        
        [SetUp]
        public void Setup()
        {
            _builder = new UnitTestDatabaseBuilder();
        }

        [TearDown]
        public void Teardown()
        {
            _builder = null;
        }
        
        [Test]
        public void should_have_default_database_name_generator()
        {
            var summary = _builder.ConfigurationSummary;
            
            Assert.AreEqual(summary.DatabaseNameGeneratorType, typeof(UtcTicksDatabaseNameGenerator).ToString());
        }

        [Test]
        public void should_set_database_name_generator()
        {
            _builder.UseNameGenerator(new GuidDatabaseNameGenerator());
            var summary = _builder.ConfigurationSummary;
            
            Assert.AreEqual(summary.DatabaseNameGeneratorType, typeof(GuidDatabaseNameGenerator).ToString());
        }

        [Test]
        public void should_set_script_directory()
        {
            _builder.WithScriptDirectory("default/sql");
            var summary = _builder.ConfigurationSummary;
            
            Assert.AreEqual("default/sql", summary.ScriptDirectory);
            Assert.True(summary.UsesScriptDirectory);
        }

        [Test]
        public void should_set_connection_string()
        {
            _builder.WithConnectionString("Server=localhost");
            var summary = _builder.ConfigurationSummary;
            
            Assert.False(summary.UsesConfiguration);
            Assert.True(summary.UsesConnectionString);
            Assert.AreEqual("Server=localhost", summary.ConnectionString);
        }

        [Test]
        public void should_set_configuration()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.con.json").Build();
            _builder.WithConfiguration(configuration);
            var summary = _builder.ConfigurationSummary;
            
            Assert.True(summary.UsesConfiguration);
            Assert.False(summary.UsesConnectionString);
            Assert.AreEqual("DefaultConnection", summary.ConnectionStringName);
        }

        [Test]
        public void should_set_configuration_and_connection_string_name()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.con.json").Build();
            _builder.WithConfiguration(configuration, "AlternateConnection");
            var summary = _builder.ConfigurationSummary;
            
            Assert.True(summary.UsesConfiguration);
            Assert.False(summary.UsesConnectionString);
            Assert.AreEqual("AlternateConnection", summary.ConnectionStringName);
        }

        [Test]
        public void should_throw_when_connection_string_not_configured()
        {
            Assert.Throws<ApplicationException>(() => _builder.Build());
        }

        [Test]
        public void should_throw_when_configuration_is_missing_name_connection()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.nocon.json").Build();
            _builder.WithConfiguration(configuration);
            
            Assert.Throws<ApplicationException>(() => _builder.Build());
        }

        [Test]
        public void should_throw_when_connection_manager_is_null()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.con.json").Build();
            _builder.WithConfiguration(configuration, "AlternateConnection");

            Assert.Throws<ApplicationException>(() => _builder.Build());
        }

        [Test]
        public void should_build()
        {
            IConnectionStringManager connectionStringManager = Substitute.For<IConnectionStringManager>();
            connectionStringManager.DefaultConnectionString.Returns("Server=localhost;database=a");
            connectionStringManager.TransientConnectionString.Returns("Server=localhost;database=b");
            IDatabase database = Substitute.For<IDatabase>();
            ISession session = Substitute.For<ISession>();
            
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.con.json").Build();
            _builder.WithConfiguration(configuration, "AlternateConnection");
            _builder.ForVendor(connectionStringManager, database, session);
            var summary = _builder.ConfigurationSummary;
            
            Assert.DoesNotThrow(() => _builder.Build());
            Assert.AreEqual("Server=localhost;database=b", summary.TransientConnectionString);
        }
    }
}