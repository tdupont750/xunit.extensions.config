using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit.Extensions.Models;
using Xunit.Extensions.Services.Base;

namespace Xunit.Extensions.Services.Implementation
{
    public class AppConfigTestDataService : ConfigTestDataServiceBase
    {
        private static readonly Regex PrefixRegex = new Regex(@"^TestData\[(\d+)\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex TestNameRegex = new Regex(@"^TestData\[(\d+)\]\.Name$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex DataRegex = new Regex(@"\.Data\[(\d+)\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex DataNameRegex = new Regex(@"\.Name$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex IndexRegex = new Regex(@"\[(\d+)\]$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private readonly Lazy<IDictionary<string, IList<DataModel<string>>>> _tests;

        public AppConfigTestDataService(NameValueCollection collection)
        {
            _tests = new Lazy<IDictionary<string, IList<DataModel<string>>>>(() => ParseTests(collection));
        }

        protected IDictionary<string, IList<DataModel<string>>> Tests => _tests.Value;

        public static bool TryCreate(out IConfigTestDataService service)
        {
            var hasKeys = ConfigurationManager.AppSettings.AllKeys.Any(k => PrefixRegex.IsMatch(k));

            if (hasKeys)
            {
                service = new AppConfigTestDataService(ConfigurationManager.AppSettings);
                return true;;
            }
            
            service = null;
            return false;
        }

        protected static Dictionary<string, IList<DataModel<string>>> ParseTests(NameValueCollection collection)
        {
            return collection.AllKeys
                .Select(key => new
                {
                    Key = key,
                    Match = PrefixRegex.Match(key)
                })
                .Where(pair => pair.Match.Success)
                .GroupBy(pair => int.Parse(pair.Match.Groups[1].Value))
                .OrderBy(testGroup => testGroup.Key)
                .ToDictionary(testGroup =>
                {
                    var nameKey = testGroup
                        .Select(pair => pair.Key)
                        .Single(key => TestNameRegex.IsMatch(key));

                    return collection[nameKey];
                }, testGroup =>
                {
                    return (IList<DataModel<string>>) testGroup
                        .Select(pair => new
                        {
                            pair.Key,
                            Match = DataRegex.Match(pair.Key)
                        })
                        .Where(pair => pair.Match.Success)
                        .GroupBy(pair => int.Parse(pair.Match.Groups[1].Value))
                        .OrderBy(dataGroup => dataGroup.Key)
                        .Select((dataGroup, index) =>
                        {
                            var nameKey = dataGroup
                                .Select(pair => pair.Key)
                                .SingleOrDefault(key => DataNameRegex.IsMatch(key));

                            var name = string.IsNullOrWhiteSpace(nameKey)
                                ? null
                                : collection[nameKey];

                            var data = dataGroup
                                .Select(pair => new
                                {
                                    pair.Key,
                                    Match = IndexRegex.Match(pair.Key)
                                })
                                .Where(pair => pair.Match.Success)
                                .Select(pair => new
                                {
                                    pair.Key,
                                    Index = int.Parse(pair.Match.Groups[1].Value)
                                })
                                .OrderBy(pair => pair.Index)
                                .Select(pair => collection[pair.Key])
                                .ToArray();

                            return new DataModel<string>
                            {
                                Index = index,
                                Name = name,
                                Data = data
                            };
                        })
                        .ToList();
                });
        }

        protected override IList<DataModel> GetDataModels(string name, IList<Type> parameterTypes)
        {
            if (!_tests.Value.ContainsKey(name))
            {
                return null;
            }

            return _tests.Value[name]
                .Select(t =>
                {
                    var converted = ConvertTypes(t.Data, parameterTypes);

                    return new DataModel
                    {
                        Index = t.Index,
                        Name = t.Name,
                        Data = converted
                    };
                })
                .ToList();
        }
    }
}