using System;
using System.Linq;
using System.Reflection;
using Xunit.Extensions.Helpers;

namespace Xunit.Extensions
{
    public class ConfigTestDataServiceTests : ConfigTestDataService
    {
        [Fact]
        public void GetDataFromConfig()
        {
            var types = new[]
            {
                typeof (string),
                typeof (int),
                typeof (bool),
                typeof (TestEnum)
            };

            var data = GetDataModels("Demo", types).ToList();

            Assert.Equal(2, data.Count);
            Assert.Equal(data[0].Data, new object[] { "Hello", 123, true, TestEnum.World });
            Assert.Equal(data[1].Data, new object[] { "Goodnight", 456, false, TestEnum.Moon });
        }

        [Fact]
        public void GetName()
        {
            var currentMethod = (MethodInfo)MethodBase.GetCurrentMethod();
            var name = GetName(currentMethod);

            Assert.Equal("Xunit.Extensions.ConfigTestDataServiceTests.GetName", name);
        }

        [Theory]
        [InlineData("A", typeof(string), "A")]
        [InlineData("123", typeof(int), 123)]
        [InlineData("456", typeof(int?), 456)]
        [InlineData("", typeof(int?), null)]
        [InlineData("true", typeof(bool), true)]
        [InlineData("Goodnight", typeof(TestEnum), TestEnum.Goodnight)]
        public void ConvertTypes(string value, Type type, object result)
        {
            var values = new[] { value };
            var types = new[] { type };
            var actualResults = ConvertTypes(values, types);
            var expectedResults = new[] { result };

            Assert.Equal(expectedResults, actualResults);
        }

        [Fact]
        public void GetDataWithConfigMultipleTimes()
        {
            GetDataWithConfigMultipleTimes(true);
        }

        private void GetDataWithConfigMultipleTimes(bool arg1)
        {
            Assert.True(arg1);

            var currentMethod = (MethodInfo)MethodBase.GetCurrentMethod();

            var data1 = GetData(currentMethod, true);
            Assert.Equal(2, data1.Count());

            var data2 = GetData(currentMethod, true);
            Assert.Equal(0, data2.Count());

            var data3 = GetData(currentMethod, false);
            Assert.Equal(2, data3.Count());

            var data4 = GetData(currentMethod, true);
            Assert.Equal(0, data4.Count());
        }
    }
}