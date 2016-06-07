using OSCforPCL.Values;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OSCforPCL
{
    public class OSCBundle : OSCPacket
    {
        private const int BUNDLE_STRING_SIZE = 8;
        private const int TIME_TAG_SIZE = 8;
        private const int MESSAGE_SIZE_SIZE = 4;

        public OSCTimeTag TimeTag { get; }
        public IEnumerable<OSCPacket> Contents { get; }
        public override byte[] Bytes { get; }

        public OSCBundle(IEnumerable<OSCPacket> contents)
        {
            Contents = contents;

            Bytes = new byte[GetByteLength()];
            MemoryStream stream = new MemoryStream(Bytes);
            OSCString bundleString = new OSCString("#bundle");
            stream.Write(bundleString.Bytes, 0, bundleString.Bytes.Length);
            OSCTimeTag timeTag = new OSCTimeTag(DateTime.Now);
            stream.Write(timeTag.Bytes, 0, timeTag.Bytes.Length);

            foreach (OSCPacket message in contents)
            {
                stream.Write(OSCInt.GetBigEndianIntBytes(message.Bytes.Length), 0, sizeof(int));
                stream.Write(message.Bytes, 0, message.Bytes.Length);
            }
        }

        public OSCBundle(params OSCPacket[] contents) : this(contents as IEnumerable<OSCPacket>)
        { }

        private int GetByteLength()
        {
            return BUNDLE_STRING_SIZE + TIME_TAG_SIZE + (Contents.Count() * MESSAGE_SIZE_SIZE) + GetMessagesBytesLength();
        }

        /// <summary>
        /// This is only the content length and DOES NOT INCLUDE the size component
        /// </summary>
        /// <returns></returns>
        private int GetMessagesBytesLength()
        {
            int length = 0;
            foreach(OSCPacket message in Contents)
            {
                length += message.Bytes.Length;
            }
            return length;
        }

        public static new OSCBundle Parse(ArraySegment<byte> bytes)
        {
            OSCString bundleString = OSCString.Parse(bytes);
            bytes = new ArraySegment<byte>(bytes.Array, bytes.Offset + bundleString.Bytes.Length, bytes.Count - bundleString.Bytes.Length);

            OSCTimeTag timeTag = OSCTimeTag.Parse(bytes);
            bytes = new ArraySegment<byte>(bytes.Array, bytes.Offset + timeTag.Bytes.Length, bytes.Count - timeTag.Bytes.Length);

            List<OSCPacket> contents = new List<OSCPacket>();
            while (bytes.Count > 0)
            {
                OSCInt size = OSCInt.Parse(bytes);
                bytes = new ArraySegment<byte>(bytes.Array, bytes.Offset + size.Bytes.Length, bytes.Count - size.Bytes.Length);
                OSCPacket packet = OSCPacket.Parse(bytes);
                bytes = new ArraySegment<byte>(bytes.Array, bytes.Offset + packet.Bytes.Length, bytes.Count - packet.Bytes.Length);
            }
            
            return new OSCBundle();
        }
    }
}
