using System;

namespace OSCforPCL
{
    internal class OSCInt : IOSCValue<int>
    {
        public int Contents { get; }
        public char TypeTag { get { return 'i'; } }
        public byte[] Bytes { get; }

        public OSCInt(int contents)
        {
            Contents = contents;
            Bytes = GetBytes();
        }

        private byte[] GetBytes()
        {
            return GetBigEndianIntBytes(Contents);
        }

        public static byte[] GetBigEndianIntBytes(int integer)
        {
            byte[] bytes = BitConverter.GetBytes(integer);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return bytes;
        }
    }
}