using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OSCforPCL
{
    public abstract class OSCPacket
    {
        public abstract byte[] Bytes { get; }

        public static OSCPacket Parse(byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            return Parse(reader);
        }

        public static OSCPacket Parse(BinaryReader reader)
        {
            if (reader.PeekChar() == '#')
            {
                // OSC Bundle
                return OSCBundle.Parse(reader);
            }
            else
            {
                return OSCMessage.Parse(reader);
            }
        }
    }
}
