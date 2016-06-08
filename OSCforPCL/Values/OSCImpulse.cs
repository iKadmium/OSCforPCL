using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSCforPCL.Values
{
    public class OSCImpulse : IOSCValue
    {
        public OSCImpulse()
        {
            Bytes = new byte[0];
        }

        public byte[] Bytes { get; }
        public char TypeTag { get { return 'I'; } }
        
        public object GetValue()
        {
            return null;
        }
    }
}
