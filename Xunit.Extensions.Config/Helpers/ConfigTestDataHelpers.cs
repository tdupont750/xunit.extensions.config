using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Xunit.Extensions.Configuration;

namespace Xunit.Extensions.Helpers
{
    public static class ConfigTestDataHelpers
    {
        public const string SectionName = "testData";

        private static readonly Type NullableType = typeof (Nullable<>);

        private static readonly ConcurrentDictionary<string, bool> NameHasDataMap = new ConcurrentDictionary<string, bool>();

        public static IEnumerable<object[]> GetData(MethodInfo methodUnderTest)
        {
            var name = GetName(methodUnderTest);

            IEnumerable<object[]> data = null;

            var parameterTypes = methodUnderTest
                .GetParameters()
                .Select(p => p.ParameterType)
                .ToList();

            var hasData = NameHasDataMap.GetOrAdd(name, k =>
            {
                data = GetDataFromConfig(name, parameterTypes);
                return data != null;
            });

            if (data != null)
                return data;

            return hasData
                ? Enumerable.Empty<object[]>()
                : null;
        }

        public static IList<object[]> GetDataFromConfig(string name, IList<Type> parameterTypes)
        {
            var section = (TestDataSection)ConfigurationManager.GetSection(SectionName);

            var test = section.Tests
                .Cast<TestElement>()
                .FirstOrDefault(t => t.Name == name);

            if (test == null)
                return null;

            var x =  test.Data
                .Cast<TestDataElement>()
                .OrderBy(d => d.Index)
                .Select(d => d.GetParams())
                .Select(p => ConvertTypes(p, parameterTypes))
                .ToList();

            return x;
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