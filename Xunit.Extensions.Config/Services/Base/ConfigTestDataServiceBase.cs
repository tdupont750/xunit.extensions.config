using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Extensions.Models;

namespace Xunit.Extensions.Helpers
{
    public abstract class ConfigTestDataServiceBase : IConfigTestDataService
    {
        private static readonly Type NullableType = typeof(Nullable<>);

        private readonly ConcurrentDictionary<string, bool> _nameHasDataMap = new ConcurrentDictionary<string, bool>();

        public IEnumerable<object[]> GetData(MethodInfo methodUnderTest, bool useCache)
        {
            var models = GetDataModels(methodUnderTest, useCache);

            return models?.Select(m => m.Data);
        }

        public IEnumerable<DataModel> GetDataModels(MethodInfo methodUnderTest, bool useCache)
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
                ? _nameHasDataMap.GetOrAdd(name, loadData)
                : loadData(name);

            if (data != null)
                return data;

            return hasData
                ? Enumerable.Empty<DataModel>()
                : null;
        }

        protected static string GetName(MethodInfo methodUnderTest)
        {
            if (methodUnderTest.DeclaringType == null)
                throw new ArgumentException("MethodUnderTest.DeclaringType is required");

            return $"{methodUnderTest.DeclaringType.Namespace}.{methodUnderTest.DeclaringType.Name}.{methodUnderTest.Name}";
        }

        protected abstract IList<DataModel> GetDataModels(string name, IList<Type> parameterTypes);

        protected virtual object[] ConvertTypes(IList<string> values, IList<Type> types)
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