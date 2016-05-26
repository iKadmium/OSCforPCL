using System;

namespace OSCforPCL
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

        public int GetByteLength()
        {
            return sizeof(float);
        }
    }
}
