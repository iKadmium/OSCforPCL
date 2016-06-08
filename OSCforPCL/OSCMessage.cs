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
                IOSCValue value = OSCValue.Wrap(obj);
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

        public static new OSCMessage Parse(BinaryReader reader)
        {
            OSCString address = OSCString.Parse(reader);
            OSCString typeTags = OSCString.Parse(reader);
            
            int valueCount = typeTags.Contents.Length - 1;
            List<IOSCValue> values = new List<IOSCValue>();

            foreach(char current in typeTags.Contents.Substring(1))
            {
                IOSCValue value;
                switch (current)
                {
                    case 'b':
                        //blob
                        value = OSCBlob.Parse(reader);
                        break;
                    case 'f':
                        //float
                        value = OSCFloat.Parse(reader);
                        break;
                    case 'i':
                        //int
                        value = OSCInt.Parse(reader);
                        break;
                    case 's':
                        //string
                        value = OSCString.Parse(reader);
                        break;
                    case 't':
                        value = OSCTimeTag.Parse(reader);
                        break;
                    default:
                        throw new ArgumentException("No such type tag as " + current);
                }

                values.Add(value);
            }

            return new OSCMessage(address.Contents, values);
        }
    }
}
