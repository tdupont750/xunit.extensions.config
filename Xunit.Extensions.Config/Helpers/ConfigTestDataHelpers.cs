using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Xunit.Extensions.Configuration;
using Xunit.Extensions.Models;

namespace Xunit.Extensions.Helpers
{
    public static class ConfigTestDataHelpers
    {
        public const string SectionName = "testData";

        private static readonly Type NullableType = typeof (Nullable<>);

        private static readonly ConcurrentDictionary<string, bool> NameHasDataMap = new ConcurrentDictionary<string, bool>();

        public static IEnumerable<object[]> GetData(MethodInfo methodUnderTest, bool useCache = true)
        {
            var models = GetDataModels(methodUnderTest, useCache);

            return models == null
                ? null
                : models.Select(m => m.Data);
        }

        public static IEnumerable<DataModel> GetDataModels(MethodInfo methodUnderTest, bool useCache = true)
        {
            var name = GetName(methodUnderTest);

            IEnumerable<DataModel> data = null;

            var loadData = new Func<string, bool>(k =>
            {
                var parameterTypes = methodUnderTest
                    .GetParameters()
                    .Select(p => p.ParameterType)
                    .ToList();

                data = GetDataModels(name, parameterTypes);
                return data != null;
            });

            var hasData = useCache 
                ? NameHasDataMap.GetOrAdd(name, loadData) 
                : loadData(name);

            if (data != null)
                return data;

            return hasData
                ? Enumerable.Empty<DataModel>()
                : null;
        }

        public static IList<DataModel> GetDataModels(string name, IList<Type> parameterTypes)
        {
            var section = (TestDataSection)ConfigurationManager.GetSection(SectionName);

            var test = section.Tests
                .Cast<TestElement>()
                .FirstOrDefault(t => t.Name == name);

            if (test == null)
                return null;

            return test.Data
                .Cast<TestDataElement>()
                .Select(d =>
                {
                    var strings = d.GetParams();
                    var converted = ConvertTypes(strings, parameterTypes);

                    return new DataModel
                    {
                        Index = d.Index,
                        Name = d.Name,
                        Data = converted
                    };
                })
                .OrderBy(m => m.Index)
                .ToList();
        }

        public static string GetName(MethodInfo methodUnderTest)
        {
            if (methodUnderTest.DeclaringType == null)
                throw new ArgumentException("MethodUnderTest.DeclaringType is required");

            return string.Format(
                "{0}.{1}.{2}",
                methodUnderTest.DeclaringType.Namespace,
                methodUnderTest.DeclaringType.Name,
                methodUnderTest.Name);
        }

        public static object[] ConvertTypes(IList<string> values, IList<Type> types)
        {
            if (values.Count != types.Count)
                throw new ArgumentException("Counts do not match");

            var results = new object[values.Count];

            for (var i = 0; i < types.Count; i++)
            {
                var type = types[i];
                var value = values[i];

                if (type.IsGenericType && type.GetGenericTypeDefinition() == NullableType)
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        results[i] = null;
                        continue;
                    }

                    type = type.GetGenericArguments().Single();
                }

                results[i] = type.IsEnum
                    ? Enum.Parse(type, value)
                    : Convert.ChangeType(value, type);
            }

            return results;
        }
    }
}