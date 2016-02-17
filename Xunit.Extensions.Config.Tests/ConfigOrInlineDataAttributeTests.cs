namespace Xunit.Extensions
{
    public class ConfigOrInlineDataAttributeTests
    {
        [Theory]
        [ConfigOrInlineData(1)]
        public void GetDataWithoutConfig(int num)
        {
            Assert.Equal(1, num);
        }

        [Theory]
        [ConfigOrInlineData(2)]
        public void GetDataWithConfig(int num)
        {
            Assert.Equal(0, num);
        }
    }
}