using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Extensions.Models;
using Xunit.Extensions.Services;
using Xunit.Extensions.Services.Implementation;

namespace Xunit.Extensions.Helpers
{
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
                    return _service ?? (_service = CreateService());
                }
            }

            // TODO - Feature Request
            // It is hard to set this before xUnit runs, so in the future we
            // should add an app setting that allows the user to provide an
            // instance of IConfigTestDataService.
            // Story: As a dev, I want to pull config from a remote data source.
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