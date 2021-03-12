using System;
using System.Collections.Generic;
using Momentary.Common;
using Momentary.Data;
using Momentary.Sqlite;
using NSubstitute;
using NUnit.Framework;

namespace Momentary.Tests.Common.Unit.Sqlite
{
    [TestFixture]
    public class DatabaseTests
    {
        private IConnectionStringManager _connectionStringManager;
        private ISession _session;

        [SetUp]
        public void Setup()
        {
            _connectionStringManager = new ConnectionStringManager();
            _session = Substitute.For<ISession>();
        }

        [TearDown]
        public void Teardown()
        {
        }

        [Test]
        public void should_throw_on_non_extant_script_directory()
        {
            var dir = $"/tmp/{RandomData.RandomWord()}";
            var database = new Database();

            Assert.Throws<ArgumentException>(() => database.RunScripts(dir));
        }

        [Test]
        public void should_call_scripts()
        {
            var defaultConnection = $"Data Source={RandomData.RandomWord()}";
            var transientDatabase = RandomData.RandomWord();
            _connectionStringManager = new ConnectionStringManager();
            _connectionStringManager.ConfigureTransientConnectionString(defaultConnection, transientDatabase);

            var database = new Database();
            database.ConfigureAndBuild(_connectionStringManager, new Dictionary<string, object>(), _session);

            database.RunScripts("../../../Common/Sql");
            _session.Received(2).Execute(Arg.Is(_connectionStringManager.TransientConnectionString),
                Arg.Any<string>());
        }
    }
}