using System.Collections.Generic;
using System.Reflection;
using Xunit.Extensions.Models;

namespace Xunit.Extensions.Services.Implementation
{
    public class EmptyConfigTestDataService : IConfigTestDataService
    {
        public IEnumerable<object[]> GetData(MethodInfo methodUnderTest, bool useCache = true)
        {
            return null;
        }

        public IEnumerable<DataModel> GetDataModels(MethodInfo methodUnderTest, bool useCache = true)
        {
            return null;
        }
    }
}