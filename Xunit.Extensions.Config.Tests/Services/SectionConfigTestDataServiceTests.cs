using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Xunit.Extensions.Configuration;
using Xunit.Extensions.Models;
using Xunit.Extensions.Services.Implementation;

namespace Xunit.Extensions.Services
{
    public class SectionConfigTestDataServiceTests : SectionConfigTestDataService
    {
        private static readonly TestDataSection Section = (TestDataSection) ConfigurationManager.GetSection(SectionName);

        public SectionConfigTestDataServiceTests() 
            : base(Section)
        {
        }

        public static void GetDataFromConfig(string s, int i, bool b, TestEnum e)
        {
        }

        [Fact]
        public void GetDataFromConfig()
        {
            var method = GetType().GetMethod("GetDataFromConfig", BindingFlags.Public | BindingFlags.Static);

            var parameters = method.GetParameters();
            var data = GetDataModels("Demo", parameters).ToList();

            Assert.Equal(2, data.Count);
            Assert.Equal(data[0].IndexedData, new object[] { "Hello", 123, true, TestEnum.World });
            Assert.Equal(data[1].IndexedData, new object[] { "Goodnight", 456, false, TestEnum.Moon });
        }

        [Fact]
        public void GetName()
        {
            var currentMethod = (MethodInfo)MethodBase.GetCurrentMethod();
            var name = GetName(currentMethod);

            Assert.Equal("Xunit.Extensions.Services.SectionConfigTestDataServiceTests.GetName", name);
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

        public static void GetDataWithConfigMultipleTimes(bool b)
        {
        }

        [Fact]
        public void GetDataWithConfigMultipleTimes()
        {
            var method = GetType().GetMethod("GetDataWithConfigMultipleTimes", BindingFlags.Public | BindingFlags.Static);

            var data1 = GetData(method, true);
            Assert.Equal(2, data1.Count());

            var data2 = GetData(method, true);
            Assert.Equal(0, data2.Count());

            var data3 = GetData(method, false);
            Assert.Equal(2, data3.Count());

            var data4 = GetData(method, true);
            Assert.Equal(0, data4.Count());
        }
    }
}