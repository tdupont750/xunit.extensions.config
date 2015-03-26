using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Xunit.Extensions.Helpers;

namespace Xunit.Extensions
{
    [Obsolete("Use ConfigOrMemberDataAttribute")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ConfigOrPropertyDataAttribute : ConfigOrMemberDataAttribute
    {
        public ConfigOrPropertyDataAttribute(string memberName, params object[] parameters)
            : base(memberName, parameters)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ConfigOrMemberDataAttribute : MemberDataAttributeBase
    {
        public ConfigOrMemberDataAttribute(string memberName, params object[] parameters)
            : base(memberName, parameters)
        {
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            return ConfigTestDataHelpers.GetData(testMethod) ?? base.GetData(testMethod);
        }

        /// <inheritdoc/>
        protected override object[] ConvertDataItem(MethodInfo testMethod, object item)
        {
            if (item == null)
                return null;

            var objArray = item as object[];
            if (objArray != null)
                return objArray;

            var message = string.Format(
                CultureInfo.CurrentCulture, 
                "Property {0} on {1} yielded an item that is not an object[]", 
                MemberName, 
                MemberType ?? testMethod.DeclaringType);

            throw new ArgumentException(message);
        }
    }
}