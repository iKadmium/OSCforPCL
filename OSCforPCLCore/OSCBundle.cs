using OSCforPCL.Values;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OSCforPCL
{
    public class OSCBundle : IOSCBundleContent
    {
        private const int BUNDLE_STRING_SIZE = 8;
        private const int TIME_TAG_SIZE = 8;
        private const int MESSAGE_SIZE_SIZE = 4;

        public OSCTimeTag TimeTag { get; }
        public IEnumerable<IOSCBundleContent> Contents { get; }
        public byte[] Bytes { get; }

        public OSCBundle(IEnumerable<IOSCBundleContent> contents)
        {
            Contents = contents;

            Bytes = new byte[GetByteLength()];
            MemoryStream stream = new MemoryStream(Bytes);
            OSCString bundleString = new OSCString("#bundle");
            stream.Write(bundleString.Bytes, 0, bundleString.Bytes.Length);
            OSCTimeTag timeTag = new OSCTimeTag(DateTime.Now);
            stream.Write(timeTag.Bytes, 0, timeTag.Bytes.Length);

            foreach (IOSCBundleContent message in contents)
            {
                stream.Write(OSCInt.GetBigEndianIntBytes(message.Bytes.Length), 0, sizeof(int));
                stream.Write(message.Bytes, 0, message.Bytes.Length);
            }
        }

        public OSCBundle(params IOSCBundleContent[] contents) : this(contents as IEnumerable<IOSCBundleContent>)
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
            foreach(IOSCBundleContent message in Contents)
            {
                length += message.Bytes.Length;
            }
            return length;
        }
    }
}
