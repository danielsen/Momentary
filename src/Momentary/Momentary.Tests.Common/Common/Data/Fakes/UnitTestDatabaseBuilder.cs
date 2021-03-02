using Momentary.Common;

namespace Momentary.Tests.Common.Common.Data.Fakes
{
    public class UnitTestDatabaseBuilder : DatabaseBuilder
    {
        public UnitTestDatabaseBuilder() : base()
        {
        }

        public DatabaseConfigurationSummary ConfigurationSummary => GetConfigurationSummary();
    }
}