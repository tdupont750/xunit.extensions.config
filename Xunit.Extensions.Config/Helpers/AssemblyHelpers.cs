using System;
using System.Reflection;

namespace Xunit.Extensions.Helpers
{
    public static class AssemblyHelpers
    {
        public static object InvokeStaticMethod(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var parts = value.Split(',');

            if (parts.Length > 2)
                return null;

            var lastPeriodIndex = parts[0].LastIndexOf('.');

            if (lastPeriodIndex < 0)
                return null;

            var typeName = parts[0].Substring(0, lastPeriodIndex);
            var methodName = parts[0].Substring(lastPeriodIndex + 1);
            var assemblyName = parts.Length == 2
                ? "," + parts[1]
                : string.Empty;

            var fullTypeName = typeName + assemblyName;
            var type = Type.GetType(fullTypeName);

            if (type == null)
                return null;

            var method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);
            return method?.Invoke(null, new object[0]);
        }
    }
}