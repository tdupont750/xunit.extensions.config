using System;

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
}