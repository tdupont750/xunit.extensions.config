using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Extensions.Helpers;
using Xunit.Sdk;

namespace Xunit.Extensions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ConfigOrInlineDataAttribute : DataAttribute
    {
        private readonly object[] _data;

        public ConfigOrInlineDataAttribute(params object[] dataValues)
        {
            _data = dataValues;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            return ConfigTestDataHelpers.GetData(testMethod) ?? new[] { _data };
        }
    }
}
