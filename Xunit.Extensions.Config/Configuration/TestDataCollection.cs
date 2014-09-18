using System.Configuration;

namespace Xunit.Extensions.Configuration
{
    [ConfigurationCollection(typeof(TestDataElement))]
    public class TestDataCollection : ConfigurationElementCollection
    {
        public TestElement this[int index]
        {
            get { return (TestElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TestDataElement)element).Index;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new TestDataElement();
        }
    }
}