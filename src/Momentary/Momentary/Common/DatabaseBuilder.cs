using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Momentary.Common
{
    public class DatabaseBuilder
    {
        /// <summary>
        /// Collection of actions to execute for database creation.
        /// </summary>
        private readonly IList<Func<IDatabase, IDatabase>> _buildSteps;

        /// <summary>
        /// Options <see cref="Microsoft.Extensions.Configuration"/> that provides
        /// a database connection string.
        /// </summary>
        private IConfiguration _configuration;

        /// <summary>
        /// Database connection string.
        /// </summary>
        private string _connectionString;
        
        /// <summary>
        /// Database connection string manager.
        /// </summary>
        private IConnectionStringManager _connectionStringManager;

        /// <summary>
        /// The connection string name to use with <see cref="_configuration"/>.
        /// </summary>
        private string _connectionStringName = "DefaultConnection";

        /// <summary>
        /// The database abstract.
        /// </summary>
        private IDatabase _database;
        
        /// <summary>
        /// Database name generator;
        /// </summary>
        private IDatabaseNameGenerator _databaseNameGenerator;

        /// <summary>
        /// Collection of arbitrary configuration properties.
        /// </summary>
        private readonly IDictionary<string, object> _properties;

        /// <summary>
        /// Database session.
        /// </summary>
        private ISession _session;

        /// <summary>
        /// Path to directory with custom SQL scripts to run after database creation/
        /// </summary>
        private string _scriptDirectory;

        public DatabaseBuilder()
        {
            _buildSteps = new List<Func<IDatabase, IDatabase>>
            {
                database =>
                {
                    database.ConfigureAndBuild(_connectionStringManager, _properties, _session);
                    return database;
                }
            };
            _databaseNameGenerator = new UtcTicksDatabaseNameGenerator();
            _properties = new Dictionary<string, object>();
        }

        private void Configure()
        {
            if (_configuration != null)
            {
                _connectionString = _configuration.GetConnectionString(_connectionStringName);
            }

            if (string.IsNullOrEmpty(_connectionString) || string.IsNullOrWhiteSpace(_connectionString))
                throw new ApplicationException("Configuration or connection string required.");
        }

        protected DatabaseConfigurationSummary GetConfigurationSummary()
        {
            var summary = new DatabaseConfigurationSummary
            {
                ConnectionString = _connectionString,
                ConnectionStringManagerType = _connectionStringManager?.GetType().ToString(),
                ConnectionStringName = _connectionStringName,
                DatabaseNameGeneratorType = _databaseNameGenerator?.GetType().ToString(),
                DatabaseType = _database?.GetType().ToString(),
                ScriptDirectory = _scriptDirectory,
                SessionType = _session?.GetType().ToString(),
                TransientConnectionString = _connectionStringManager?.TransientConnectionString,
                UsesConfiguration = _configuration != null,
                UsesConnectionString = _configuration == null,
                UsesScriptDirectory = _scriptDirectory != null
            };
            return summary;
        }

        public IDatabase Build()
        {
            Configure();
            var databaseName = _databaseNameGenerator.Generate();

            if (_connectionStringManager == null)
                throw new ApplicationException($"An IConnectionStringManager must be configured.");
            
            _connectionStringManager.ConfigureTransientConnectionString(_connectionString, databaseName);
            
            foreach (var buildStep in _buildSteps)
            {
                buildStep(_database);
            }

            return _database;
        }

        public DatabaseBuilder ForVendor(IConnectionStringManager connectionStringManager, IDatabase database, ISession session)
        {
            _connectionStringManager = connectionStringManager;
            _database = database;
            _session = session;

            return this;
        }

        public DatabaseBuilder UseNameGenerator(IDatabaseNameGenerator value)
        {
            _databaseNameGenerator = value;
            
            return this;
        }

        public DatabaseBuilder WithConfiguration(IConfiguration value, string connectionStringName = null)
        {
            _configuration = value;
            if (!string.IsNullOrEmpty(connectionStringName) && !string.IsNullOrWhiteSpace(connectionStringName))
            {
                _connectionStringName = connectionStringName;
            }

            return this;
        }

        public DatabaseBuilder WithConnectionString(string value)
        {
            _connectionString = value;

            return this;
        }

        public DatabaseBuilder WithScriptDirectory(string value)
        {
            _scriptDirectory = value;
            _buildSteps.Add(database =>
            {
                database.RunScripts(_scriptDirectory);
                return database;
            });

            return this;
        }

        public string TransientConnectionString => _connectionStringManager.TransientConnectionString;
    }
    
    #region DatabaseConfigurationSummary

    public class DatabaseConfigurationSummary
    {
        public string ConnectionString { get; set; }
        public string ConnectionStringManagerType { get; set; }
        public string ConnectionStringName { get; set; }
        public string DatabaseNameGeneratorType { get; set; }
        public string DatabaseType { get; set; }
        public string ScriptDirectory { get; set; }
        public string SessionType { get; set; }
        public string TransientConnectionString { get; set; }
        public bool UsesConfiguration { get; set; }
        public bool UsesConnectionString { get; set; }
        public bool UsesScriptDirectory { get; set; }
    }
    
    #endregion
}