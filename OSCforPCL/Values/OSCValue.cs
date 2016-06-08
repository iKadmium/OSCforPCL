using OSCforPCL.Values;
using System;
using System.Collections.Generic;
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
    }
}
