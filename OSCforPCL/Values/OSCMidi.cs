using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OSCforPCL.Values
{
    public class MidiMessage
    {
        public byte PortID { get; }
        public byte Status { get; }
        public byte Data1 { get; }
        public byte Data2 { get; }

        public MidiMessage(byte portID, byte status, byte data1, byte data2)
        {
            PortID = portID;
            Status = status;
            Data1 = data1;
            Data2 = data2;
        }
    }

    public class OSCMidi : IOSCValue<MidiMessage>
    {
        public OSCMidi(byte portID, byte status, byte data1, byte data2) : this(new MidiMessage(portID, status, data1, data2))
        {}

        public OSCMidi(MidiMessage midiMessage)
        {
            Contents = midiMessage;
            Bytes = new byte[] { Contents.PortID, Contents.Status, Contents.Data1, Contents.Data2 };
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(Bytes);
            }
        }

        public byte[] Bytes { get; }
        public MidiMessage Contents { get; }

        public char TypeTag { get { return 'm'; } }

        public object GetValue()
        {
            return Contents;
        }

        public static OSCMidi Parse(BinaryReader reader)
        {
            byte[] bytes = reader.ReadBytes(4);
            if(BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return new OSCMidi(bytes[0], bytes[1], bytes[2], bytes[3]);
        }
    }
}
