using Momentary.Common;
using NUnit.Framework;

namespace Momentary.Tests.Common.Unit.Common
{
    [TestFixture]
    public class GuidDatabaseNameGeneratorTests
    {
        [Test]
        public void should_generate_database_name()
        {
            var generator = new GuidDatabaseNameGenerator();
            var result = generator.Generate();
            
            Assert.True(result.Contains("momentary"));
        }
    }
}