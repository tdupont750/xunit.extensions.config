using System.Collections.Generic;
using System.Reflection;
using Xunit.Extensions.Models;

namespace Xunit.Extensions
{
    public interface IConfigDataAttribute
    {
        IEnumerable<DataModel> GetDataModels(MethodInfo testMethod);
    }
}