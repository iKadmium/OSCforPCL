using OSCforPCL.Values;
using System;
using System.IO;

namespace OSCforPCL.Values
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

        public static OSCBlob Parse(ArraySegment<byte> bytes)
        {
            MemoryStream stream = new MemoryStream(bytes.Array, bytes.Offset, bytes.Count);
            BinaryReader reader = new BinaryReader(stream);
            int size = reader.ReadInt32();
            byte[] blobBytes = new byte[size];
            Array.Copy(bytes.Array, bytes.Offset + sizeof(Int32), blobBytes, 0, size);
            return new OSCBlob(blobBytes);
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
