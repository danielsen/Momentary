using Momentary.MySql;
using Momentary.Tests.Common.Common.Data.Fakes;
using NUnit.Framework;

namespace Momentary.Tests.Common.Unit.MySql
{
    [TestFixture]
    public class DatabaseBuilderExtensionTests
    {
        private UnitTestDatabaseBuilder _builder;

        [SetUp]
        public void Setup()
        {
            _builder = new UnitTestDatabaseBuilder();
        }

        [TearDown]
        public void Teardown()
        {
            _builder = null;
        }

        [Test]
        public void should_have_correct_vendor()
        {
            _builder = new UnitTestDatabaseBuilder();
            _builder.ForMySql();

            var summary = _builder.ConfigurationSummary;
            Assert.AreEqual(summary.ConnectionStringManagerType, typeof(ConnectionStringManager).ToString());
            Assert.AreEqual(summary.DatabaseType, typeof(Database).ToString());
            Assert.AreEqual(summary.SessionType, typeof(Session).ToString());
        }
    }
}