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

        private static readonly object ServiceLock = new object();
        
        private static IConfigTestDataService _service;

        public static IConfigTestDataService Service
        {
            get
            {
                lock (ServiceLock)
                {
                    if (_service != null)
                        return _service;
                    
                    return _service = CreateService();
                }
            }
        }

        public static void SetService(IConfigTestDataService service)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            lock (ServiceLock)
            {
                if (_service != null)
                    throw new InvalidOperationException("IConfigTestDataService already set");

                _service = service;
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

        private static IConfigTestDataService CreateService()
        {
            IConfigTestDataService service;

            if (ConfigurationManager.AppSettings.AllKeys.Contains(InitKey))
            {
                var value = ConfigurationManager.AppSettings[InitKey];

                service = AssemblyHelpers.InvokeStaticMethod(value) as IConfigTestDataService;

                if (service == null)
                    throw new ConfigurationErrorsException(InitKey + " did not return an IConfigTestDataService");

                return service;
            }

            if (SectionConfigTestDataService.TryCreate(out service))
                return service;

            if (AppConfigTestDataService.TryCreate(out service))
                return service;

            return new EmptyConfigTestDataService();
        }
    }
}