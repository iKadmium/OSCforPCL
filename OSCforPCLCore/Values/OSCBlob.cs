using System;

namespace OSCforPCL
{
    class OSCBlob : IOSCValue<byte[]>
    {
        public static int PaddingLength = 4;

        public byte[] Contents { get; }
        public char TypeTag { get { return 'b'; } }
        public byte[] Bytes { get; }

        public OSCBlob(byte[] contents)
        {
            Contents = contents;
            Bytes = GetBytes();
        }

        private byte[] GetBytes()
        {
            byte[] returnValue = new byte[GetByteLength()];
            Array.Copy(BitConverter.GetBytes(Contents.Length) , returnValue, sizeof(int));
            Array.Copy(Contents, 0, returnValue, sizeof(Int32), Contents.Length);
            return returnValue;
        }

        public int GetByteLength()
        {
            return GetPaddedLength(Contents.Length);
        }

        public static int GetPaddedLength(int length)
        {
            int terminatedLength = length + 1;
            int paddingRequired = PaddingLength - (terminatedLength % PaddingLength);
            return length + paddingRequired;
        }
    }
}
