using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Xunit.Extensions.Configuration
{
    public class TestDataElement : ConfigurationElement
    {
        private static readonly Regex IndexedParamRegex = new Regex(@"^p(\d+)$", RegexOptions.Compiled);

        private readonly SortedList<int, string> _indexData = new SortedList<int, string>();

        private readonly Dictionary<string, string> _namedData = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        
        private const string IndexName = "index";

        [ConfigurationProperty(IndexName, IsKey = true, IsRequired = true)]
        public int Index
        {
            get { return (int)this[IndexName]; }
            set { this[IndexName] = value; }
        }

        private const string NameName = "name";

        [ConfigurationProperty(NameName, IsKey = false, IsRequired = false)]
        public string Name
        {
            get { return (string)this[NameName]; }
            set { this[NameName] = value; }
        }

        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            var match = IndexedParamRegex.Match(name);
            if (match.Success)
            {
                var i = int.Parse(match.Groups[1].Value);
                _indexData.Add(i, value);
            }
            else
            {
                _namedData.Add(name, value);
            }

            return true;
        }
        
        public IList<string> GetData(IList<ParameterInfo> parameters)
        {
            if (_indexData.Count != 0 ^ _namedData.Count != 0)
            {
                return _indexData.Count > 0
                    ? _indexData.Values
                    : parameters.Select(p => _namedData[p.Name]).ToArray();
            }

            throw new InvalidOperationException("Unique indexed or named data not detected for " + Name);
        }
    }
}