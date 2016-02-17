using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Extensions.Models;
using Xunit.Sdk;

namespace Xunit.Extensions
{
    public static class DataAttributeExtensions
    {
        public static IEnumerable<DataModel> GetDataModels(this DataAttribute attribute, MethodInfo methodUnderTest)
        {
            var configDataAttribute = attribute as IConfigDataAttribute;
            if (configDataAttribute != null)
                return configDataAttribute.GetDataModels(methodUnderTest);

            var data = attribute.GetData(methodUnderTest);
            return data.ToDataModels();
        }

        public static IEnumerable<DataModel> ToDataModels(this IEnumerable<object[]> data)
        {
            return data?.Select((d, i) => new DataModel
            {
                Index = i,
                Name = string.Empty,
                Data = d
            });
        }
    }
}
