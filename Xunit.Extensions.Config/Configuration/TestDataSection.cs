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

        private const string DefaultNamespaceName = "defaultNamespace";

        [ConfigurationProperty(DefaultNamespaceName, IsKey = false, IsRequired = false)]
        public string DefaultNamespace
        {
            get { return (string)this[DefaultNamespaceName]; }
            set { this[DefaultNamespaceName] = value; }
        }
    }
}