using Xunit.Extensions.Helpers;

namespace Xunit.Extensions
{
    public class AssemblyHelperTests
    {
        private const int Key = 123;

        public static int CallMeMaybe()
        {
            return Key;
        }

        [Fact]
        public void InitTest()
        {
            var result = (int)AssemblyHelpers.InvokeStaticMethod("Xunit.Extensions.AssemblyHelperTests.CallMeMaybe, Xunit.Extensions.Config.Tests");
            Assert.Equal(Key, result);
        }
    }
}