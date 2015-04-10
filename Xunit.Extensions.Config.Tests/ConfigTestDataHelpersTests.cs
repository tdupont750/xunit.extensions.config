using System;
using System.Linq;
using System.Reflection;
using Xunit.Extensions.Helpers;

namespace Xunit.Extensions
{
    public class ConfigTestDataHelpersTests
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

            var data = ConfigTestDataHelpers
                .GetDataModels("Demo", types)
                .ToList();

            Assert.Equal(2, data.Count);
            Assert.Equal(data[0].Data, new object[] { "Hello", 123, true, TestEnum.World });
            Assert.Equal(data[1].Data, new object[] { "Goodnight", 456, false, TestEnum.Moon });
        }

        [Fact]
        public void GetName()
        {
            var currentMethod = (MethodInfo)MethodBase.GetCurrentMethod();
            var name = ConfigTestDataHelpers.GetName(currentMethod);

            Assert.Equal("Xunit.Extensions.ConfigTestDataHelpersTests.GetName", name);
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
            var actualResults = ConfigTestDataHelpers.ConvertTypes(values, types);
            var expectedResults = new[] { result };

            Assert.Equal(expectedResults, actualResults);
        }
    }
}