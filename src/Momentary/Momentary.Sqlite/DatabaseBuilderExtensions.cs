using Momentary.Common;

namespace Momentary.Sqlite
{
    public static class DatabaseBuilderExtensions
    {
        public static DatabaseBuilder ForSqlite(this DatabaseBuilder builder)
        {
            var connectionStringManager = new ConnectionStringManager();
            var session = new Session();

            return builder;
        }
    }
}