using System;
using System.IO;
using System.Text;
using System.Linq;

namespace OSCforPCL.Values
{
    public class OSCString : IOSCValue<string>
    {
        static int PaddingLength = 4;

        public string Contents { get; }
        public char TypeTag { get { return 's'; }}
        public byte[] Bytes { get; }

        public OSCString(string contents)
        {
            Contents = contents;
            Bytes = GetBytes();
        }

        public byte[] GetBytes()
        {
            byte[] bytes = new byte[GetByteLength()];
            Encoding.ASCII.GetBytes(Contents, 0, Contents.Length, bytes, 0);
            return bytes;
        }

        public int GetByteLength()
        {
            return GetPaddedLength(Contents.Length);
        }

        public static int GetPaddedLength(int length)
        {
            int terminatedLength = length + 1;
            int paddingRequired = PaddingLength - (terminatedLength % PaddingLength);
            if(paddingRequired == PaddingLength)
            {
                paddingRequired = 0;
            }
            return terminatedLength + paddingRequired;
        }

        public static OSCString Parse(ArraySegment<byte> bytes)
        {
            var goodChars = bytes.TakeWhile(x => x != 0);
            StringBuilder builder = new StringBuilder();
            string str = Encoding.ASCII.GetString(bytes.Array, bytes.Offset, goodChars.Count());
            return new OSCString(str);
        }
    }
}
