using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Xunit.Extensions.Models;
using Xunit.Extensions.Services;
using Xunit.Extensions.Services.Implementation;

namespace Xunit.Extensions.Helpers
{
    public static class ConfigTestDataHelpers
    {
        private const string InitKey = "TestData.ServiceFactory";

        private static readonly object InitLock = new object();

        private static readonly object ServiceLock = new object();

        private static volatile bool _isInitialized;

        private static IConfigTestDataService _service;

        public static IConfigTestDataService Service
        {
            get
            {
                TryInit();

                lock (ServiceLock)
                {
                    return _service ?? (_service = CreateService());
                }
            }

            set
            {
                lock (ServiceLock)
                {
                    if (_service != null)
                        throw new InvalidOperationException("IConfigTestDataService already set");

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

        private static void TryInit()
        {
            if (_isInitialized)
                return;

            lock (InitLock)
            {
                if (_isInitialized)
                    return;

                _isInitialized = true;

                if (!ConfigurationManager.AppSettings.AllKeys.Contains(InitKey))
                    return;

                var value = ConfigurationManager.AppSettings[InitKey];
                var service = AssemblyHelpers.InvokeStaticMethod(value) as IConfigTestDataService;

                if (service == null)
                    throw new ConfigurationErrorsException(InitKey + " did not return an IConfigTestDataService");

                Service = service;
            }
        }

        private static IConfigTestDataService CreateService()
        {
            IConfigTestDataService service;

            if (SectionConfigTestDataService.TryCreate(out service))
                return service;

            if (AppConfigTestDataService.TryCreate(out service))
                return service;

            return new EmptyConfigTestDataService();
        }
    }
}