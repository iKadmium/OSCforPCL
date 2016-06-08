using OSCforPCL;
using OSCforPCL.Values;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace test
{
    public class OSCMessageTester
    {
        [InlineData("/address", new object[] { 0, "stuff", 34f, new byte[] { 3, 5, 2 } } )]
        [Theory]
        public void TestParsing(string address, object[] data)
        {
            OSCMessage message = new OSCMessage(address, data);
            byte[] bytes = message.Bytes;

            OSCMessage parsed = OSCMessage.Parse(new BinaryReader(new MemoryStream(bytes)));
            Assert.Equal(address, parsed.Address.Contents);
            for(int i = 0; i < data.Length; i++)
            {
                Assert.Equal(data[i], parsed.Arguments[i].GetValue());
            }
        }
    }
}
