using System;
using System.Data.SqlClient;
using Momentary.Common;

namespace Momentary.Sql
{
    internal class ConnectionStringManager : IConnectionStringManager
    {
        public void ConfigureTransientConnectionString(string defaultConnectionString, string transientDatabaseName)
        {
            if (string.IsNullOrEmpty(defaultConnectionString) || string.IsNullOrWhiteSpace(defaultConnectionString))
                throw new ArgumentException($"{nameof(defaultConnectionString)} cannot be null or empty.");

            if (string.IsNullOrEmpty(transientDatabaseName) || string.IsNullOrWhiteSpace(transientDatabaseName))
                throw new ArgumentException($"{nameof(transientDatabaseName)} cannot be null or empty.");

            var connectionStringBuilder = new SqlConnectionStringBuilder(defaultConnectionString)
            {
                DataSource = transientDatabaseName
            };
            DefaultConnectionString = defaultConnectionString;
            TransientConnectionString = connectionStringBuilder.ToString();
            TransientDatabaseName = transientDatabaseName;
        }

        public string DefaultConnectionString { get; private set; }
        public string TransientDatabaseName { get; private set; }
        public string TransientConnectionString { get; private set; }
    }
}