using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using OSCforPCL.Values;

namespace test
{
    public class OSCBlobTester
    {
        [InlineData(new byte[] { 2, 3, 5 })]
        [Theory]
        public void TestOSCBlobParsing(byte[] values)
        {
            byte[] array = new byte[values.Length + sizeof(Int32)];
            Array.Copy(BitConverter.GetBytes(values.Length), array, sizeof(Int32));
            Array.Copy(values, 0, array, sizeof(Int32), values.Length);
            
            Assert.Equal(array, OSCValueTester.TestOSCValueParser(array, (reader) => OSCBlob.Parse(reader)));
        }
    }
}
