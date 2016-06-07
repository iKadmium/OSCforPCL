using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSCforPCL
{
    public abstract class OSCPacket
    {
        public abstract byte[] Bytes { get; }

        public static OSCPacket Parse(byte[] bytes)
        {
            return Parse(new ArraySegment<byte>(bytes));
        }

        public static OSCPacket Parse(ArraySegment<byte> bytes)
        {
            if (bytes.Array[bytes.Offset] == '#')
            {
                // OSC Bundle
                return OSCBundle.Parse(bytes);
            }
            else
            {
                return OSCMessage.Parse(bytes);
            }
        }
    }
}
