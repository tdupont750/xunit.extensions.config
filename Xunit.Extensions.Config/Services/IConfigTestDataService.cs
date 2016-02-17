using System.Collections.Generic;
using System.Reflection;
using Xunit.Extensions.Models;

namespace Xunit.Extensions.Helpers
{
    public interface IConfigTestDataService
    {
        IEnumerable<object[]> GetData(MethodInfo methodUnderTest, bool useCache = true);

        IEnumerable<DataModel> GetDataModels(MethodInfo methodUnderTest, bool useCache = true);
    }
}