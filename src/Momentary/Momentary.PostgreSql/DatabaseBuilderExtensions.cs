using Momentary.Common;

namespace Momentary.PostgreSql
{
    public static class DatabaseBuilderExtensions
    {
        public static DatabaseBuilder ForPgsql(this DatabaseBuilder builder)
        {
            var connectionStringManager = new ConnectionStringManager();
            var database = new Database();
            var session = new Session();

            builder.ForVendor(connectionStringManager, database, session);
            return builder;
        }
    }
}