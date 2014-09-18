using System.Configuration;

namespace Xunit.Extensions.Configuration
{
    public class TestElement : ConfigurationElement
    {
        private const string AttributeName = "name";

        [ConfigurationProperty(AttributeName, IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this[AttributeName]; }
            set { this[AttributeName] = value; }
        }

        private const string DataName = "data";

        [ConfigurationProperty(DataName, IsKey = false, IsRequired = true)]
        public TestDataCollection Data
        {
            get { return (TestDataCollection)this[DataName]; }
            set { this[DataName] = value; }
        }
    }
}