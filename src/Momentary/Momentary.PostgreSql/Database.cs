using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Momentary.Common;

namespace Momentary.PostgreSql
{
    internal class Database : IDatabase
    {
        private IConnectionStringManager _connectionStringManager;
        private ISession _session;
        
        public string DefaultConnectionString => _connectionStringManager.DefaultConnectionString;
        public string TemplateDatabase { get; private set; }
        public string TransientConnectionString => _connectionStringManager.TransientConnectionString;

        private string BuildCreateStatement()
        {
            StringBuilder stringBuilder = new StringBuilder($"create database {_connectionStringManager.TransientDatabaseName}");

            if (!string.IsNullOrWhiteSpace(TemplateDatabase))
            {
                stringBuilder.Append($" template {TemplateDatabase}");
            }

            stringBuilder.Append(';');
            return stringBuilder.ToString();
        }
        
        public void ConfigureAndBuild(IConnectionStringManager connectionStringManager, IDictionary<string, object> properties,
            ISession session)
        {
            _connectionStringManager = connectionStringManager;
            
            if (properties != null && properties.ContainsKey("TemplateDatabase"))
                TemplateDatabase = (string) properties["TemplateDatabase"];

            _session = session;
            
            _session.Execute(_connectionStringManager.DefaultConnectionString, BuildCreateStatement());
        }

        public void Drop()
        {
            _session.Execute(_connectionStringManager.DefaultConnectionString,
                $"select pid, pg_terminate_backend(pid) from pg_stat_activity where datname = '{_connectionStringManager.TransientDatabaseName}' and pid <> pg_backend_pid();");
            _session.Execute(_connectionStringManager.DefaultConnectionString,
                $"drop database {_connectionStringManager.TransientDatabaseName}");
        }

        public bool Exists()
        {
            _session.ExecuteReader(_connectionStringManager.DefaultConnectionString,
                $"select 1 as result from pg_database where datname = '{_connectionStringManager.TransientDatabaseName}'",
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