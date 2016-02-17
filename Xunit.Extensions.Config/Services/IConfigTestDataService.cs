namespace Xunit.Extensions.Services
{
    using System.Collections.Generic;
    using System.Reflection;

    using Xunit.Extensions.Models;

    public interface IConfigTestDataService
    {
        IEnumerable<object[]> GetData(MethodInfo methodUnderTest, bool useCache = true);

        IEnumerable<DataModel> GetDataModels(MethodInfo methodUnderTest, bool useCache = true);
    }
}