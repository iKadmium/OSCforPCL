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
        [InlineData("/address", new object[] { 0, "stuff", 34f, true, null, new byte[] { 3, 5, 2 } } )]
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

        [InlineData("/impulseTest")]
        [Theory]
        public void TestImpulseMessage(string address)
        {
            OSCImpulse impulse = new OSCImpulse();
            OSCMessage message = new OSCMessage(address, impulse);
            byte[] bytes = message.Bytes;

            OSCMessage parsed = OSCMessage.Parse(new BinaryReader(new MemoryStream(bytes)));
            Assert.Equal(address, parsed.Address.Contents);
            Assert.True(parsed.Arguments.Count == 1);
            Assert.True(parsed.Arguments[0].GetType() == typeof(OSCImpulse));
        }

        [InlineData("/newOSCTypeTest")]
        [Theory]
        public void TestNewOSCTypes(string address)
        {
            OSCColor color = new OSCColor(255, 0, 0, 255);
            OSCMidi midi = new OSCMidi(0, 0, 127, 0);
            OSCMessage message = new OSCMessage(address, color, midi);
            byte[] bytes = message.Bytes;

            OSCMessage parsed = OSCMessage.Parse(new BinaryReader(new MemoryStream(bytes)));
            Assert.Equal(address, parsed.Address.Contents);
            Assert.True(parsed.Arguments.Count == 2);
            Assert.True(parsed.Arguments[0].GetType() == typeof(OSCColor));
            Assert.True((parsed.Arguments[0] as OSCColor).Contents.Red == 255);
            Assert.True(parsed.Arguments[1].GetType() == typeof(OSCMidi));
            Assert.True((parsed.Arguments[1] as OSCMidi).Contents.Data1 == 127);
        }
    }
}
