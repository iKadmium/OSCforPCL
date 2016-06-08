using OSCforPCL.Values;
using System;
using System.IO;

namespace OSCforPCL.Values
{
    public class OSCInt : IOSCValue<int>
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

        public static OSCInt Parse(BinaryReader reader)
        {
            byte[] intBytes = reader.ReadBytes(sizeof(Int32));
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(intBytes);
            }
            int value = BitConverter.ToInt32(intBytes, 0);
            return new OSCInt(value);
        }

        public object GetValue()
        {
            return Contents;
        }

        public override string ToString()
        {
            return Contents.ToString();
        }
    }
}