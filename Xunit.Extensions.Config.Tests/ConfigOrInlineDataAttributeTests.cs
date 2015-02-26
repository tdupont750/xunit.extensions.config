﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Xunit.Extensions
{
    public class ConfigOrInlineDataAttributeTests
    {
        private static readonly IList<int> GetDataWithoutConfigList = new List<int>();

        [Theory]
        [ConfigOrInlineData(1)]
        [ConfigOrInlineData(2)]
        public void GetDataWithoutConfig(int num)
        {
            GetDataWithoutConfigList.Add(num);

            Assert.Equal(GetDataWithoutConfigList.Count, num);
        }

        private static readonly IList<int> GetDataWithConfigList = new List<int>();

        [Theory]
        [ConfigOrInlineData(3)]
        [ConfigOrInlineData(4)]
        public void GetDataWithConfig(int num)
        {
            GetDataWithConfigList.Add(num);

            Assert.Equal(GetDataWithConfigList.Count, num);
        }

        [Fact]
        public void GetDataWithConfigTwice()
        {
            var attribute = new ConfigOrInlineDataAttribute();
            var currentMethod = (MethodInfo) MethodBase.GetCurrentMethod();
            var types = new[] {typeof (bool)};

            var data1 = attribute.GetData(currentMethod, types);
            Assert.Equal(2, data1.Count());

            var data2 = attribute.GetData(currentMethod, types);
            Assert.Equal(0, data2.Count());
        }
    }
}