using OSCforPCL.Values;
using System;
using System.Collections.Generic;
using System.IO;

namespace OSCforPCL
{
    public class OSCMessage : OSCPacket
    {
        public OSCString Address { get; set; }
        public List<IOSCValue> Arguments { get; set; }
        public override byte[] Bytes { get; }

        public OSCMessage(string address, params object[] values)
        {
            Address = new OSCString(address);
            Arguments = new List<IOSCValue>();
            foreach (object obj in values)
            {
                IOSCValue value = Wrap(obj);
                Arguments.Add(value);
            }
            Bytes = GetBytes();
        }

        public OSCMessage(string address, IEnumerable<IOSCValue> values)
        {
            Address = new OSCString(address);
            Arguments = new List<IOSCValue>(values);
            Bytes = GetBytes();
        }

        private IOSCValue Wrap(object obj)
        {
            if(obj.GetType() == typeof(byte[]))
            {
                return new OSCBlob(obj as byte[]);
            }
            else if(obj.GetType() == typeof(string))
            {
                return new OSCString(obj as string);
            }
            else if (obj.GetType() == typeof(int))
            {
                return new OSCInt((int)obj);
            }
            else if (obj.GetType() == typeof(float))
            {
                return new OSCFloat((float)obj);
            }
            else
            {
                throw new ArgumentException(obj.GetType() + " is not a legal OSC Value type");
            }
        }

        private byte[] GetBytes()
        {
            byte[] bytes = new byte[GetByteLength()];
            MemoryStream stream = new MemoryStream(bytes);
            stream.Write(Address.Bytes, 0, Address.Bytes.Length);
            OSCString typeTag = GetTypeTagString();
            stream.Write(typeTag.Bytes, 0, typeTag.Bytes.Length);
            foreach(IOSCValue value in Arguments)
            {
                stream.Write(value.Bytes, 0, value.Bytes.Length);
            }
            return bytes;
        }

        public int GetByteLength()
        {
            return Address.GetByteLength() + GetTypeTagLength() + GetArgumentsLength();
        }

        private OSCString GetTypeTagString()
        {
            char[] chars = new char[Arguments.Count + 1];
            chars[0] = ',';
            int index = 1;
            foreach (IOSCValue value in Arguments)
            {
                chars[index] = value.TypeTag;
                index++;
            }
            return new OSCString(new string(chars));
        }

        private int GetTypeTagLength()
        {
            return OSCString.GetPaddedLength(Arguments.Count + 2);
        }

        private int GetArgumentsLength()
        {
            int length = 0;
            foreach(IOSCValue value in Arguments)
            {
                length += value.Bytes.Length;
            }
            return length;
        }

        public static new OSCMessage Parse(ArraySegment<byte> bytes)
        {
            OSCString address = OSCString.Parse(bytes);
            bytes = new ArraySegment<byte>(bytes.Array, bytes.Offset + address.Bytes.Length, bytes.Count - address.Bytes.Length);
            OSCString typeTags = OSCString.Parse(bytes);
            bytes = new ArraySegment<byte>(bytes.Array, bytes.Offset + typeTags.Bytes.Length, bytes.Count - typeTags.Bytes.Length);

            int valueCount = typeTags.Contents.Length - 1;
            List<IOSCValue> values = new List<IOSCValue>();

            foreach(char current in typeTags.Contents.Substring(1))
            {
                IOSCValue value;
                switch (current)
                {
                    case 'b':
                        //blob
                        value = OSCBlob.Parse(bytes);
                        break;
                    case 'f':
                        //float
                        value = OSCFloat.Parse(bytes);
                        break;
                    case 'i':
                        //int
                        value = OSCInt.Parse(bytes);
                        break;
                    case 's':
                        //string
                        value = OSCString.Parse(bytes);
                        break;
                    case 't':
                        value = OSCTimeTag.Parse(bytes);
                        break;
                    default:
                        throw new ArgumentException("No such type tag as " + current);
                }

                values.Add(value);
                bytes = new ArraySegment<byte>(bytes.Array, bytes.Offset + value.Bytes.Length, bytes.Count - value.Bytes.Length);
            }

            return new OSCMessage(address.Contents, values);
        }
    }
}
