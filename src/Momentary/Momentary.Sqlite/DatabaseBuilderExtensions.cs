using Momentary.Common;

namespace Momentary.Sqlite
{
    public static class DatabaseBuilderExtensions
    {
        public static DatabaseBuilder ForSqlite(this DatabaseBuilder builder)
        {
            var connectionStringManager = new ConnectionStringManager();
            var database = new Database();
            var session = new Session();

            builder.ForVendor(connectionStringManager, database, session);
            return builder;
        }
    }
}