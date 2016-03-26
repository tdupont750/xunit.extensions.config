using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using Xunit.Extensions.Models;
using Xunit.Extensions.Services.Implementation;

namespace Xunit.Extensions.Services
{
    public class AppConfigTestDataServiceTests : AppConfigTestDataService
    {
        private static readonly NameValueCollection Settings = new NameValueCollection
        {
            { "Hello", "World" },

            { "TestData.DefaultNamespace", "Xunit.Extensions.Services" },

            { "TestData[0].Name", "A" },
            { "TestData[0].Data[0].Name", "A0" },
            { "TestData[0].Data[0][0]", "A00" },

            { "TestData[1].Name", "B" },
            { "TestData[1].Data[1][3]", "B12" },
            { "TestData[1].Data[1][2]", "B11" },
            { "TestData[1].Data[1][1]", "B10" },
            { "TestData[1].Data[0][0]", "B00" },

            { "TestData[2].Name", "C" },
            { "TestData[2].Data[0][B1]", "C0B1" },
            { "TestData[2].Data[0][A0]", "C0A0" },
            { "TestData[2].Data[1][A]", "C1A" },
            { "TestData[2].Data[1][B]", "C1B" },
            { "TestData[2].Data[1][C]", "C1C" },

            { "TestData[3].Name", "AppConfigTestDataServiceTests.GetNamedFromPairs" },
            { "TestData[3].Data[0][i]", "42" },
            { "TestData[3].Data[0][am]", "true" },
            { "TestData[3].Data[0][testing]", "Hello" },

            { "Goodnight", "Moon" },
        };

        public AppConfigTestDataServiceTests()
            : base(Settings)
        {
        }

        public static void GetNamedFromPairs(TestEnum testing, int i, bool am)
        {
        }

        [Fact]
        public void GetNamedFromPairs()
        {
            var method = GetType().GetMethod("GetNamedFromPairs", BindingFlags.Static | BindingFlags.Public);
            var data = GetData(method, false).Single();

            Assert.Equal(TestEnum.Hello, (TestEnum)data[0]);
            Assert.Equal(42, (int)data[1]);
            Assert.Equal(true, (bool)data[2]);
        }

        [Fact]
        public void GetTests()
        {
            Assert.Equal(4, Tests.Count);

            Assert.True(Tests.ContainsKey("A"));
            Assert.Equal(1, Tests["A"].Count);

            Assert.Equal("A0", Tests["A"][0].Name);
            Assert.Equal(0, Tests["A"][0].Index);
            Assert.Equal(1, Tests["A"][0].IndexedData.Length);
            Assert.Equal("A00", Tests["A"][0].IndexedData[0]);

            Assert.True(Tests.ContainsKey("B"));
            Assert.Equal(2, Tests["B"].Count);

            Assert.Null(Tests["B"][0].Name);
            Assert.Equal(0, Tests["B"][0].Index);
            Assert.Equal(1, Tests["B"][0].IndexedData.Length);
            Assert.Equal("B00", Tests["B"][0].IndexedData[0]);

            Assert.Null(Tests["B"][1].Name);
            Assert.Equal(1, Tests["B"][1].Index);
            Assert.Equal(3, Tests["B"][1].IndexedData.Length);
            Assert.Equal("B10", Tests["B"][1].IndexedData[0]);
            Assert.Equal("B11", Tests["B"][1].IndexedData[1]);
            Assert.Equal("B12", Tests["B"][1].IndexedData[2]);

            Assert.True(Tests.ContainsKey("C"));
            Assert.Equal(2, Tests["C"].Count);

            Assert.Null(Tests["C"][0].Name);
            Assert.Equal(0, Tests["C"][0].Index);
            Assert.Equal(2, Tests["C"][0].NamedData.Count);
            Assert.Equal("C0B1", Tests["C"][0].NamedData["B1"]);
            Assert.Equal("C0A0", Tests["C"][0].NamedData["A0"]);

            Assert.Null(Tests["C"][0].Name);
            Assert.Equal(1, Tests["C"][1].Index);
            Assert.Equal(3, Tests["C"][1].NamedData.Count);
            Assert.Equal("C1A", Tests["C"][1].NamedData["A"]);
            Assert.Equal("C1B", Tests["C"][1].NamedData["B"]);
            Assert.Equal("C1C", Tests["C"][1].NamedData["C"]);
        }
    }
}