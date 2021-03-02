using System.Collections.Generic;

namespace Momentary.Common
{
    /// <summary>
    /// Represents a transient database.
    /// </summary>
    public interface IDatabase 
    {
        /// <summary>
        /// Configure and build the database from the provided default connection string.
        /// </summary>
        /// <param name="connectionStringManager">Implementation specific connection string manager.</param>
        /// <param name="properties">Arbitrary configuration properties.</param>
        void ConfigureAndBuild(IConnectionStringManager connectionStringManager, IDictionary<string, object> properties,
            ISession session);
        
        /// <summary>
        /// Drop the transient database.
        /// </summary>
        void Drop();
        
        /// <summary>
        /// Check that the database exists.
        /// </summary>
        /// <returns></returns>
        bool Exists();
        
        /// <summary>
        /// Execute custom SQL scripts/
        /// </summary>
        /// <param name="directory">SQL script directory.</param>
        void RunScripts(string directory);
    }
}