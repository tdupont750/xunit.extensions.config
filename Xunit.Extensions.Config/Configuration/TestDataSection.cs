using System.Configuration;

namespace Xunit.Extensions.Configuration
{
    public class TestDataSection : ConfigurationSection
    {
        private const string TestsName = "tests";

        [ConfigurationProperty(TestsName)]
        public TestCollection Tests
        {
            get { return (TestCollection)base[TestsName]; }
        }
    }
}