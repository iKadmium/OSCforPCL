using OSCforPCL.Values;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OSCforPCL.Values
{
    public static class OSCValue
    {
        public static IOSCValue Wrap(object obj)
        {
            if(obj == null)
            {
                return new OSCNull();
            }
            else if (obj.GetType() == typeof(byte[]))
            {
                return new OSCBlob(obj as byte[]);
            }
            else if (obj.GetType() == typeof(string))
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
            else if(obj.GetType() == typeof(DateTime))
            {
                return new OSCTimeTag((DateTime)obj);
            }
            else if(obj.GetType() == typeof(Color))
            {
                return new OSCColor(obj as Color);
            }
            else if(obj.GetType() == typeof(MidiMessage))
            {
                return new OSCMidi(obj as MidiMessage);
            }
            else if(obj.GetType() == typeof(bool))
            {
                bool value = (bool)obj;
                if(value)
                {
                    return new OSCTrue();
                }
                else
                {
                    return new OSCFalse();
                }
            }
            else
            {
                throw new ArgumentException(obj.GetType() + " is not a legal OSC Value type");
            }
        }

        public static IOSCValue Parse(char typeTag, BinaryReader reader)
        {
            switch (typeTag)
            {
                case 'i':
                    return OSCInt.Parse(reader);
                case 'f':
                    return OSCFloat.Parse(reader);
                case 's':
                    return OSCString.Parse(reader);
                case 'b':
                    return OSCBlob.Parse(reader);
                case 'T':
                    return new OSCTrue();
                case 'F':
                    return new OSCFalse();
                case 'N':
                    return new OSCNull();
                case 'I':
                    return new OSCImpulse();
                case 't':
                    return OSCTimeTag.Parse(reader);
                case 'c':
                    return OSCColor.Parse(reader);
                case 'm':
                    return OSCMidi.Parse(reader);
                default:
                    throw new ArgumentException("No such type tag as " + typeTag);
            }
        }
    }
}
