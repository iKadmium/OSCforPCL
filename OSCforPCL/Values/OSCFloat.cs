using OSCforPCL.Values;
using System;
using System.IO;

namespace OSCforPCL.Values
{
    public class OSCFloat : IOSCValue<float>
    {
        public float Contents { get; }
        public char TypeTag { get { return 'f'; } }
        public byte[] Bytes { get; }

        public OSCFloat(float contents)
        {
            Contents = contents;
            Bytes = GetBytes();
        }

        private byte[] GetBytes()
        {
            byte[] bytes = BitConverter.GetBytes(Contents);
            if(BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return bytes;
        }

        public static OSCFloat Parse(BinaryReader reader)
        {
            byte[] floatBytes = reader.ReadBytes(sizeof(float));

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(floatBytes);
            }
            float value = BitConverter.ToSingle(floatBytes, 0);
            return new OSCFloat(value);
        }

        public int GetByteLength()
        {
            return sizeof(float);
        }
    }
}
