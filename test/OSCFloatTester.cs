using OSCforPCL.Values;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace test
{
    public class OSCFloatTester
    {
        [InlineData(0f)]
        [InlineData(float.MinValue)]
        [InlineData(float.MaxValue)]
        [InlineData(float.NegativeInfinity)]
        [InlineData(float.PositiveInfinity)]
        [InlineData(float.NaN)]
        [Theory]
        public void TestParse(float value)
        {
            Assert.Equal(value, OSCValueTester.TestOSCValueParser(value, (reader) => OSCFloat.Parse(reader)));
        }
    }
}
