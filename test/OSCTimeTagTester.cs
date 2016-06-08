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
            DateTime time = new DateTime(year, month, day, hour, minute, second, millisecond);
            OSCTimeTag tag = new OSCTimeTag(time);
            byte[] bytes = tag.Bytes;

            OSCTimeTag parsed = OSCTimeTag.Parse(new BinaryReader(new MemoryStream(bytes)));
            Assert.Equal(time, parsed.Contents);
        }
    }
}
