namespace Momentary.Common
{
    /// <summary>
    /// Manages connection strings for temporary databases.
    /// </summary>
    public interface IConnectionStringManager
    {
        /// <summary>
        /// Sets the default connection string and transient database name to the manager implementation.
        /// </summary>
        /// <param name="defaultConnectionString">Default database connection string.</param>
        /// <param name="transientDatabaseName">Name of the transient database.</param>
        void ConfigureTransientConnectionString(string defaultConnectionString, string transientDatabaseName); 
        
        /// <summary>
        /// The default connection string.
        /// </summary>
        string DefaultConnectionString { get; }
        
        /// <summary>
        /// The name of the transient database.
        /// </summary>
        string TransientDatabaseName { get; }
        
        /// <summary>
        /// The transient connection string.
        /// </summary>
        string TransientConnectionString { get; }
    }
}