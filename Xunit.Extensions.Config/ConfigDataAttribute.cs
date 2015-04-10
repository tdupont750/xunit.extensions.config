using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Extensions.Helpers;
using Xunit.Extensions.Models;
using Xunit.Sdk;

namespace Xunit.Extensions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ConfigDataAttribute : DataAttribute, IConfigDataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            return ConfigTestDataHelpers.GetData(testMethod);
        }

        public IEnumerable<DataModel> GetDataModels(MethodInfo testMethod)
        {
            return ConfigTestDataHelpers.GetDataModels(testMethod);
        }
    }
}