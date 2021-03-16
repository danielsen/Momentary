using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Momentary.Common;

namespace Momentary.Sql
{
    internal class Database : IDatabase
    {
        private IConnectionStringManager _connectionStringManager;
        private ISession _session;

        public void ConfigureAndBuild(IConnectionStringManager connectionStringManager,
            IDictionary<string, object> properties, ISession session)
        {
            _connectionStringManager = connectionStringManager;
            _session = session;

            var createStatement = $"CREATE DATABASE {_connectionStringManager.TransientDatabaseName};";
            _session.Execute(_connectionStringManager.DefaultConnectionString, createStatement);
        }

        public void Drop()
        {
            _session.Execute(_connectionStringManager.DefaultConnectionString,
                $"DROP DATABASE {_connectionStringManager.TransientDatabaseName};");
        }

        public bool Exists()
        {
            _session.ExecuteReader(_connectionStringManager.DefaultConnectionString,
                $"SELECT 1 AS [Result] WHERE DB_ID(N'{_connectionStringManager.TransientDatabaseName}') IS NOT NULL;",
                out int rows);
            return rows > 0;
        }

        public void RunScripts(string directory)
        {
            if (!Directory.Exists(directory))
                throw new ArgumentException($"Directory does not exist: {directory}");

            Directory.GetFiles(directory).OrderBy(s => s).Select(File.ReadAllText).ToList()
                .ForEach(command => _session.Execute(_connectionStringManager.TransientConnectionString, command));
        }
    }
}