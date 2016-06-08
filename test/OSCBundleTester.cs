using OSCforPCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace test
{
    public class OSCBundleTester
    {
        [Fact]
        public void TestParse()
        {
            OSCMessage first = new OSCMessage("/address1", 1, 2, 3, "string");
            OSCMessage second = new OSCMessage("/address2", 3, 4, 5, "otherstring");

            OSCBundle bundle = new OSCBundle(first, second);

            OSCBundle parsed = OSCBundle.Parse(new BinaryReader(new MemoryStream(bundle.Bytes)));

            Assert.Equal(first.Bytes, parsed.Contents[0].Bytes);
            Assert.Equal(second.Bytes, parsed.Contents[1].Bytes);
        }
    }
}
