using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Xunit.Extensions.Models;
using Xunit.Extensions.Services.Base;

namespace Xunit.Extensions.Services.Implementation
{
    public class AppConfigTestDataService : ConfigTestDataServiceBase
    {
        private static readonly Regex PrefixRegex = new Regex(@"^TestData\[(\d+)\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex TestNameRegex = new Regex(@"^TestData\[(\d+)\]\.Name$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex DataRegex = new Regex(@"\.Data\[([a-z0-9]+)\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex DataNameRegex = new Regex(@"\.Name$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex IndexedRegex = new Regex(@"\[(\d+)\]$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex NamedRegex = new Regex(@"\[([a-z0-9]+)\]$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

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

                            var dataMatches = dataGroup
                                .Select(pair => new 
                                {
                                    pair.Key,
                                    IndexedMatch = IndexedRegex.Match(pair.Key),
                                    NamedMatch = NamedRegex.Match(pair.Key)
                                })
                                .ToArray();

                            var indexData = dataMatches
                                .Where(pair => pair.IndexedMatch.Success)
                                .Select(pair => new
                                {
                                    pair.Key,
                                    Index = int.Parse(pair.IndexedMatch.Groups[1].Value)
                                })
                                .OrderBy(pair => pair.Index)
                                .Select(pair => collection[pair.Key])
                                .ToArray();

                            var namedData = dataMatches
                                .Where(pair => !pair.IndexedMatch.Success && pair.NamedMatch.Success)
                                .ToDictionary(key => key.NamedMatch.Groups[1].Value, value => collection[value.Key], StringComparer.InvariantCultureIgnoreCase);

                            if (indexData.Length != 0 ^ namedData.Count != 0)
                            {
                                return new DataModel<string>
                                {
                                    Index = index,
                                    Name = name,
                                    IndexedData = indexData,
                                    NamedData = namedData
                                };
                            }

                            throw new InvalidOperationException("Unique indexed or named data not detected for " + name);
                        })
                        .ToList();
                });
        }

        protected override IList<DataModel> GetDataModels(string name, IList<ParameterInfo> parameters)
        {
            if (!_tests.Value.ContainsKey(name))
            {
                return null;
            }

            return _tests.Value[name]
                .Select(t =>
                {
                    object[] converted;

                    var paramTypes = parameters.Select(p => p.ParameterType).ToArray();

                    if (t.IndexedData.Length > 0)
                    {
                        converted = ConvertTypes(t.IndexedData, paramTypes);
                    }
                    else if (t.NamedData.Count > 0)
                    {
                        var values = parameters.Select(p => t.NamedData[p.Name]).ToArray();
                        converted = ConvertTypes(values, paramTypes);
                    }
                    else
                    {
                        throw new InvalidOperationException("No data found for " + name);
                    }

                    return new DataModel
                    {
                        Index = t.Index,
                        Name = t.Name,
                        IndexedData = converted
                    };
                })
                .ToList();
        }
    }
}