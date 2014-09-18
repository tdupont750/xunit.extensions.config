using System.Collections.Generic;
using System.Configuration;

namespace Xunit.Extensions.Configuration
{
    public class TestDataElement : ConfigurationElement
    {
        private const string IndexName = "index";

        [ConfigurationProperty(IndexName, IsKey = true, IsRequired = true)]
        public int Index
        {
            get { return (int)this[IndexName]; }
            set { this[IndexName] = value; }
        }

        private const string P0Name = "p0";

        [ConfigurationProperty(P0Name, IsKey = false, IsRequired = false)]
        public string P0
        {
            get { return (string)this[P0Name]; }
            set { this[P0Name] = value; }
        }

        private const string P1Name = "p1";

        [ConfigurationProperty(P1Name, IsKey = false, IsRequired = false)]
        public string P1
        {
            get { return (string)this[P1Name]; }
            set { this[P1Name] = value; }
        }

        private const string P2Name = "p2";

        [ConfigurationProperty(P2Name, IsKey = false, IsRequired = false)]
        public string P2
        {
            get { return (string)this[P2Name]; }
            set { this[P2Name] = value; }
        }

        private const string P3Name = "p3";

        [ConfigurationProperty(P3Name, IsKey = false, IsRequired = false)]
        public string P3
        {
            get { return (string)this[P3Name]; }
            set { this[P3Name] = value; }
        }

        private const string P4Name = "p4";

        [ConfigurationProperty(P4Name, IsKey = false, IsRequired = false)]
        public string P4
        {
            get { return (string)this[P4Name]; }
            set { this[P4Name] = value; }
        }

        private const string P5Name = "p5";

        [ConfigurationProperty(P5Name, IsKey = false, IsRequired = false)]
        public string P5
        {
            get { return (string)this[P5Name]; }
            set { this[P5Name] = value; }
        }

        private const string P6Name = "p6";

        [ConfigurationProperty(P6Name, IsKey = false, IsRequired = false)]
        public string P6
        {
            get { return (string)this[P6Name]; }
            set { this[P6Name] = value; }
        }

        private const string P7Name = "p7";

        [ConfigurationProperty(P7Name, IsKey = false, IsRequired = false)]
        public string P7
        {
            get { return (string)this[P7Name]; }
            set { this[P7Name] = value; }
        }

        private const string P8Name = "p8";

        [ConfigurationProperty(P8Name, IsKey = false, IsRequired = false)]
        public string P8
        {
            get { return (string)this[P8Name]; }
            set { this[P8Name] = value; }
        }

        private const string P9Name = "p9";

        [ConfigurationProperty(P9Name, IsKey = false, IsRequired = false)]
        public string P9
        {
            get { return (string)this[P9Name]; }
            set { this[P9Name] = value; }
        }

        public IList<string> GetParams()
        {
            var results = new List<string>();

            for (var i = 0; i < 10; i++)
            {
                var p = (string) this["p" + i];

                if (!string.IsNullOrWhiteSpace(p))
                    results.Add(p);
            }

            return results;
        }
    }
}