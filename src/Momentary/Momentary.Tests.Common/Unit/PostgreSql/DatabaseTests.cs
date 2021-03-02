using System;
using System.Collections.Generic;
using Momentary.Common;
using Momentary.Data;
using Momentary.PostgreSql;
using NSubstitute;
using NUnit.Framework;

namespace Momentary.Tests.Common.Unit.PostgreSql
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
        public void should_call_correct_create_statement()
        {
            var defaultConnection = $"Server=localhost;Database={RandomData.RandomWord()}";
            var transientDatabase = RandomData.RandomWord();

            _connectionStringManager = new ConnectionStringManager();
            _connectionStringManager.ConfigureTransientConnectionString(defaultConnection, transientDatabase);

            var database = new Database();
            database.ConfigureAndBuild(_connectionStringManager, new Dictionary<string, object>(), _session);

            _session.Received(1).Execute(Arg.Is(defaultConnection),
                Arg.Is($"create database {transientDatabase};"));
        }

        [Test]
        public void should_call_correct_create_statement_with_template()
        {
            var defaultConnection = $"Server=localhost;Database={RandomData.RandomWord()}";
            var transientDatabase = RandomData.RandomWord();
            var templateDatabase = RandomData.RandomWord();
            var properties = new Dictionary<string, object>()
            {
                {"TemplateDatabase", templateDatabase}
            };

            _connectionStringManager = new ConnectionStringManager();
            _connectionStringManager.ConfigureTransientConnectionString(defaultConnection, transientDatabase);

            var database = new Database();
            database.ConfigureAndBuild(_connectionStringManager, properties, _session);

            _session.Received(1).Execute(Arg.Is(defaultConnection),
                Arg.Is($"create database {transientDatabase} template {templateDatabase};"));
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
            var defaultConnection = $"Server=localhost;Database={RandomData.RandomWord()}";
            var transientDatabase = RandomData.RandomWord();
            _connectionStringManager = new ConnectionStringManager();
            _connectionStringManager.ConfigureTransientConnectionString(defaultConnection, transientDatabase);

            var database = new Database();
            database.ConfigureAndBuild(_connectionStringManager, new Dictionary<string, object>(), _session);

            database.RunScripts("../../../Common/Sql");
            _session.Received(2).Execute(Arg.Is(_connectionStringManager.TransientConnectionString),
                Arg.Any<string>());
        }

        [Test]
        public void should_call_correct_drop_statements()
        {
            var defaultConnection = $"Server=localhost;Database={RandomData.RandomWord()}";
            var transientDatabase = RandomData.RandomWord();
            _connectionStringManager = new ConnectionStringManager();
            _connectionStringManager.ConfigureTransientConnectionString(defaultConnection, transientDatabase);

            var database = new Database();
            database.ConfigureAndBuild(_connectionStringManager, new Dictionary<string, object>(), _session);

            database.Drop();

            _session.Received(1).Execute(Arg.Is(_connectionStringManager.DefaultConnectionString),
                Arg.Is(
                    $"select pid, pg_terminate_backend(pid) from pg_stat_activity where datname = '{_connectionStringManager.TransientDatabaseName}' and pid <> pg_backend_pid();"));
            _session.Received(1).Execute(Arg.Is(_connectionStringManager.DefaultConnectionString),
                Arg.Is($"drop database {_connectionStringManager.TransientDatabaseName}"));
        }

        [Test]
        public void should_call_correct_exists_statements()
        {
            var defaultConnection = $"Server=localhost;Database={RandomData.RandomWord()}";
            var transientDatabase = RandomData.RandomWord();
            _connectionStringManager = new ConnectionStringManager();
            _connectionStringManager.ConfigureTransientConnectionString(defaultConnection, transientDatabase);

            var database = new Database();
            database.ConfigureAndBuild(_connectionStringManager, new Dictionary<string, object>(), _session);
            database.Exists();
            
            _session.Received(1).ExecuteReader(Arg.Is(_connectionStringManager.DefaultConnectionString),
                Arg.Is($"select 1 as result from pg_database where datname = '{_connectionStringManager.TransientDatabaseName}'"),
                out int x);            
        }
    }
}