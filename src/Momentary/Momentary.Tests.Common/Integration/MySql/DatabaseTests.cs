using Momentary.Data;
using Momentary.MySql;
using Momentary.Tests.Common.Common;
using NUnit.Framework;

namespace Momentary.Tests.Common.Integration.MySql
{
    [TestFixture]
    public class DatabaseTests
    {
        [Test]
        public void should_handle_database_setup_and_teardown()
        {
            var configuration = new Configuration().Current.GetSection("ConnectionStrings");
            var transientDatabaseName = RandomData.RandomWord();

            var connectionStringManager = new ConnectionStringManager();
            connectionStringManager.ConfigureTransientConnectionString(configuration["MySQL"], transientDatabaseName);

            var database = new Database();
            database.ConfigureAndBuild(connectionStringManager, null, new Session());

            Assert.True(database.Exists());

            database.Drop();

            Assert.False(database.Exists());
        }
    }
}