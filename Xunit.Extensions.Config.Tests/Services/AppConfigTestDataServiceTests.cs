using System.Collections.Specialized;
using Xunit.Extensions.Services.Implementation;

namespace Xunit.Extensions.Services
{
    public class AppConfigTestDataServiceTests : AppConfigTestDataService
    {
        private static readonly NameValueCollection Settings = new NameValueCollection
        {
            { "Hello", "World" },
            { "TestData[0].Name", "A" },
            { "TestData[0].Data[0].Name", "A0" },
            { "TestData[0].Data[0][0]", "A00" },
            { "TestData[1].Name", "B" },
            { "TestData[1].Data[1][3]", "B12" },
            { "TestData[1].Data[1][2]", "B11" },
            { "TestData[1].Data[1][1]", "B10" },
            { "TestData[1].Data[0][0]", "B00" },
            { "Goodnight", "Moon" },
        };

        public AppConfigTestDataServiceTests()
            : base(Settings)
        {
        }

        [Fact]
        public void GetTests()
        {
            Assert.Equal(2, Tests.Count);

            Assert.True(Tests.ContainsKey("A"));
            Assert.Equal(1, Tests["A"].Count);

            Assert.Equal("A0", Tests["A"][0].Name);
            Assert.Equal(0, Tests["A"][0].Index);
            Assert.Equal(1, Tests["A"][0].Data.Length);
            Assert.Equal("A00", Tests["A"][0].Data[0]);

            Assert.True(Tests.ContainsKey("B"));
            Assert.Equal(2, Tests["B"].Count);

            Assert.Null(Tests["B"][0].Name);
            Assert.Equal(0, Tests["B"][0].Index);
            Assert.Equal(1, Tests["B"][0].Data.Length);
            Assert.Equal("B00", Tests["B"][0].Data[0]);

            Assert.Null(Tests["B"][1].Name);
            Assert.Equal(1, Tests["B"][1].Index);
            Assert.Equal(3, Tests["B"][1].Data.Length);
            Assert.Equal("B10", Tests["B"][1].Data[0]);
            Assert.Equal("B11", Tests["B"][1].Data[1]);
            Assert.Equal("B12", Tests["B"][1].Data[2]);
        }
    }
}