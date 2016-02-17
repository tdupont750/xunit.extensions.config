using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Extensions.Models;

namespace Xunit.Extensions.Helpers
{
    using Xunit.Extensions.Services;
    using Xunit.Extensions.Services.Implementation;

    public static class ConfigTestDataHelpers
    {
        private static readonly object ServiceLock = new object();

        private static IConfigTestDataService _service;

        public static IConfigTestDataService Service
        {
            get
            {
                lock (ServiceLock)
                {
                    return _service ?? (_service = new ConfigTestDataService());
                }
            }
            set
            {
                lock (ServiceLock)
                {
                    if (_service != null)
                    {
                        throw new InvalidOperationException("IConfigTestDataService already set");
                    }

                    _service = value;
                }
            }
        }

        public static IEnumerable<object[]> GetData(MethodInfo methodUnderTest)
        {
            return Service.GetData(methodUnderTest);
        }

        public static IEnumerable<DataModel> GetDataModels(MethodInfo methodUnderTest)
        {
            return Service.GetDataModels(methodUnderTest);
        }
    }
}