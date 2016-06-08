using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OSCforPCL.Values
{
    public class Color
    {
        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }
        public byte Alpha { get; }

        public Color(byte red, byte green, byte blue, byte alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }
    }

    public class OSCColor : IOSCValue<Color>
    {
        public OSCColor(byte red, byte green, byte blue, byte alpha) : this(new Color(red, green, blue, alpha))
        {
        }

        public OSCColor(Color color)
        {
            Contents = color;
            Bytes = new byte[4] { Contents.Red, Contents.Green, Contents.Blue, Contents.Alpha };
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(Bytes);
            }
        }

        public byte[] Bytes { get; }
        public char TypeTag { get { return 'c'; } }

        public Color Contents { get; }

        public object GetValue()
        {
            return Contents;
        }

        public static OSCColor Parse(BinaryReader reader)
        {
            byte red = reader.ReadByte();
            byte green = reader.ReadByte();
            byte blue = reader.ReadByte();
            byte alpha = reader.ReadByte();
            return new OSCColor(red, green, blue, alpha);
        }
    }
}
