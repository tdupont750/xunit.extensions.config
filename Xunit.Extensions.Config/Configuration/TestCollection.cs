using System.Configuration;

namespace Xunit.Extensions.Configuration
{
    [ConfigurationCollection(typeof(TestElement))]
    public class TestCollection : ConfigurationElementCollection
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
            return ((TestElement)element).Name;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new TestElement();
        }
    }
}