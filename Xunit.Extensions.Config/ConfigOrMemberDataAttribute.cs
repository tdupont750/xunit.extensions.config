using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Xunit.Extensions.Helpers;
using Xunit.Extensions.Models;

namespace Xunit.Extensions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ConfigOrMemberDataAttribute : MemberDataAttributeBase, IConfigDataAttribute
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

        public IEnumerable<DataModel> GetDataModels(MethodInfo testMethod)
        {
            return ConfigTestDataHelpers.GetDataModels(testMethod) ?? base.GetData(testMethod).ToDataModels();
        }
    }
}