using OSCforPCL.Values;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace test
{
    public class OSCTimeTagTester
    {
        [InlineData(2014, 01, 05, 13, 01, 15, 254)]
        [Theory]
        public void TestParsing(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            DateTime value = new DateTime(year, month, day, hour, minute, second, millisecond);
            Assert.Equal(value, OSCValueTester.TestOSCValueParser(value, (reader) => OSCTimeTag.Parse(reader)));
        }
    }
}
