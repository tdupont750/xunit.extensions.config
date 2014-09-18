using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Extensions.Helpers;

namespace Xunit.Extensions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ConfigOrInlineDataAttribute : InlineDataAttribute
    {
        public ConfigOrInlineDataAttribute(params object[] dataValues)
            : base(dataValues)
        {
        }

        public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest, Type[] parameterTypes)
        {
            return ConfigTestDataHelpers.GetData(methodUnderTest, parameterTypes) ?? base.GetData(methodUnderTest, parameterTypes);
        }
    }
}
