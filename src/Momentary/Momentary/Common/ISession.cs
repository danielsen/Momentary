namespace Momentary.Common
{
    /// <summary>
    /// Database session.
    /// </summary>
    public interface ISession
    {
        /// <summary>
        /// Executes a single command in the database session.
        /// </summary>
        /// <param name="connectionString">The session connection string.</param>
        /// <param name="command">Command to execute</param>
        void Execute(string connectionString, string command);

        /// <summary>
        /// Executes a reader command in the database session.
        /// </summary>
        /// <param name="connectionString">The session connection string.</param>
        /// <param name="command">Command to execute.</param>
        /// <param name="rows">Row count result.</param>
        void ExecuteReader(string connectionString, string command, out int rows);
    }
}