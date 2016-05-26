﻿using System.Text;

namespace OSCforPCL
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
    }
}
