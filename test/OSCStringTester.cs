using OSCforPCL.Values;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace test
{
    public class OSCStringTester
    {
        [Fact]
        public void TestPadding()
        {
            OSCString str = new OSCString("test");
            Assert.Equal(8, str.Bytes.Length);

            str = new OSCString("#bundle");
            Assert.Equal(8, str.Bytes.Length);

            str = new OSCString("");
            Assert.Equal(4, str.Bytes.Length);
        }

        [InlineData("test")]
        [InlineData("stuff")]
        [InlineData("a particularly long string")]
        [Theory]
        public void TestParsing(string contents)
        {
            OSCString str = new OSCString(contents);
            byte[] bytes = str.Bytes;

            OSCString testString = OSCString.Parse(new BinaryReader(new MemoryStream(bytes)));
            Assert.Equal(contents, testString.Contents);
        }
    }
}
