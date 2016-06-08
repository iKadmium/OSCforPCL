using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OSCforPCL.Values
{
    public class OSCFalse : IOSCValue<bool>
    {
        public OSCFalse()
        {
            Bytes = new byte[0];
        }

        public byte[] Bytes { get; }
        public bool Contents { get { return false; } }
        public char TypeTag { get { return 'F'; } }

        public object GetValue()
        {
            return false;
        }

        public override string ToString()
        {
            return "False";
        }

        public static OSCFalse Parse(BinaryReader reader)
        {
            return new OSCFalse();
        }
    }
}
