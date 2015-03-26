using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Extensions.Helpers;
using Xunit.Sdk;

namespace Xunit.Extensions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ConfigDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            return ConfigTestDataHelpers.GetData(testMethod);
        }
    }
}