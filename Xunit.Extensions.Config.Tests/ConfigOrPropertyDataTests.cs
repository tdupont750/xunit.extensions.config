using System.Collections.Generic;

namespace Xunit.Extensions
{
    public class ConfigOrPropertyDataTests
    {
        public static IEnumerable<object[]> Prop => new []
        {
            new object[] { 1 }
        };

        [Theory]
        [ConfigOrPropertyData("Prop")]
        public void GetDataWithoutConfig(int i)
        {
            Assert.Equal(1, i);
        }
    }
}