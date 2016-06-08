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
        public List<OSCPacket> Contents { get; }
        public override byte[] Bytes { get; }

        public OSCBundle(DateTime time, IEnumerable<OSCPacket> contents)
        {
            Contents = new List<OSCPacket>(contents);

            Bytes = new byte[GetByteLength()];
            MemoryStream stream = new MemoryStream(Bytes);
            OSCString bundleString = new OSCString("#bundle");
            stream.Write(bundleString.Bytes, 0, bundleString.Bytes.Length);
            TimeTag = new OSCTimeTag(time);
            stream.Write(TimeTag.Bytes, 0, TimeTag.Bytes.Length);

            foreach (OSCPacket message in contents)
            {
                stream.Write(OSCInt.GetBigEndianIntBytes(message.Bytes.Length), 0, sizeof(int));
                stream.Write(message.Bytes, 0, message.Bytes.Length);
            }
        }

        public OSCBundle(DateTime time, params OSCPacket[] contents) : this(time, contents as IEnumerable<OSCPacket>)
        { }

        public OSCBundle(params OSCPacket[] contents) : this(DateTime.Now, contents as IEnumerable<OSCPacket>)
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

        public static new OSCBundle Parse(BinaryReader reader)
        {
            OSCString bundleString = OSCString.Parse(reader);
            OSCTimeTag timeTag = OSCTimeTag.Parse(reader);
            
            List<OSCPacket> contents = new List<OSCPacket>();
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                OSCInt size = OSCInt.Parse(reader);
                OSCPacket packet = OSCPacket.Parse(reader);
                contents.Add(packet);
            }
            
            return new OSCBundle(timeTag.Contents, contents);
        }
    }
}
