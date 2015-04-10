using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Extensions.Helpers;
using Xunit.Extensions.Models;
using Xunit.Sdk;

namespace Xunit.Extensions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ConfigOrInlineDataAttribute : DataAttribute, IConfigDataAttribute
    {
        private readonly IEnumerable<object[]> _data;

        public ConfigOrInlineDataAttribute(params object[] dataValues)
        {
            _data = new[] {dataValues};
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            return ConfigTestDataHelpers.GetData(testMethod) ?? _data ;
        }

        public IEnumerable<DataModel> GetDataModels(MethodInfo testMethod)
        {
            return ConfigTestDataHelpers.GetDataModels(testMethod) ?? _data.ToDataModels();
        }
    }
}
