using OSCforPCL.Values;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace test
{
    public class OSCValueTester
    {
        public static T TestOSCValueParser<T>(T value, Func<BinaryReader, IOSCValue<T>> parser)
        {
            IOSCValue<T> val = OSCValue.Wrap(value) as IOSCValue<T>;
            BinaryReader reader = new BinaryReader(new MemoryStream(val.Bytes));
            IOSCValue<T> parsed = parser.Invoke(reader);
            Assert.Equal(reader.BaseStream.Position, reader.BaseStream.Length);
            return parsed.Contents;
        }
    }
}
