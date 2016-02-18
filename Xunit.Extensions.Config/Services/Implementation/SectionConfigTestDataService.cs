namespace Xunit.Extensions.Services.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using Xunit.Extensions.Configuration;
    using Xunit.Extensions.Models;
    using Xunit.Extensions.Services.Base;

    public class SectionConfigTestDataService : ConfigTestDataServiceBase
    {
        protected const string SectionName = "testData";

        private readonly TestDataSection _section;

        public SectionConfigTestDataService(TestDataSection section)
        {
            _section = section;
        }

        public static bool TryCreate(out IConfigTestDataService service)
        {
            var section = ConfigurationManager.GetSection(SectionName) as TestDataSection;

            if (section == null)
            {
                service = null;
                return false;
            }

            service = new SectionConfigTestDataService(section);
            return true;
        }

        protected override IList<DataModel> GetDataModels(string name, IList<Type> parameterTypes)
        {
            var test = _section.Tests
                .Cast<TestElement>()
                .FirstOrDefault(t => t.Name == name);

            return test?.Data
                .Cast<TestDataElement>()
                .Select(d =>
                {
                    var strings = d.GetParams();
                    var converted = ConvertTypes(strings, parameterTypes);

                    return new DataModel
                    {
                        Index = d.Index,
                        Name = d.Name,
                        Data = converted
                    };
                })
                .OrderBy(m => m.Index)
                .ToList();
        }
    }
}