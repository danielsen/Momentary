using System;
using Momentary.Data;
using NUnit.Framework;
using Momentary.Sqlite;

namespace Momentary.Tests.Common.Unit.Sqlite
{
    [TestFixture]
    public class ConnectionStringManagerTests
    {
        private string _connectionString;
        private string _transientDatabaseName;

        [SetUp]
        public void Setup()
        {
            _connectionString = $"Data Source={RandomData.RandomWord()}";
            _transientDatabaseName = RandomData.RandomWord();
        }

        [Test]
        public void should_configure_connection_string_correctly()
        {
            var connectionStringManager = new ConnectionStringManager();
            connectionStringManager.ConfigureTransientConnectionString(_connectionString, _transientDatabaseName);

            Assert.NotNull(connectionStringManager.DefaultConnectionString);
            Assert.True(connectionStringManager.TransientConnectionString.Contains(_transientDatabaseName));
            Assert.AreEqual(_transientDatabaseName, connectionStringManager.TransientDatabaseName);
        }

        [Test]
        public void should_throw_when_connection_string_is_null_or_empty(
            [Values(" ", "", null)] string defaultConnection)
        {
            var connectionStringManager = new ConnectionStringManager();
            Assert.Throws<ArgumentException>(() => connectionStringManager
                .ConfigureTransientConnectionString(defaultConnection, _transientDatabaseName));
        }

        [Test]
        public void should_throw_when_transient_name_is_null_or_empty([Values(" ", "", null)] string transientName)
        {
            var connectionStringManager = new ConnectionStringManager();
            Assert.Throws<ArgumentException>(() => connectionStringManager
                .ConfigureTransientConnectionString(_connectionString, transientName));
        }
    }
}