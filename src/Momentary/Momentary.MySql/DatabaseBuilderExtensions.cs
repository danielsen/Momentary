using Momentary.Common;

namespace Momentary.MySql
{
    public static class DatabaseBuilderExtensions
    {
        public static DatabaseBuilder ForMySql(this DatabaseBuilder builder)
        {
            var connectionStringManager = new ConnectionStringManager();
            var database = new Database();
            var session = new Session();

            builder.ForVendor(connectionStringManager, database, session);
            return builder;
        }
    }
}