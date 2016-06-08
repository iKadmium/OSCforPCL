using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OSCforPCL.Values
{
    public class OSCTrue : IOSCValue<bool>
    {
        public OSCTrue()
        {
            Bytes = new byte[0];
        }

        public byte[] Bytes { get; }
        public bool Contents { get { return true; } }
        public char TypeTag { get { return 'T'; } }

        public object GetValue()
        {
            return true;
        }

        public override string ToString()
        {
            return "True";
        }
        
    }
}
