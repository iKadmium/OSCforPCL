using System;
using System.IO;

namespace OSCforPCL.Values
{
    public class OSCTimeTag : IOSCValue<DateTime>
    {
        public static DateTime BeginningOfTime = new DateTime(1900, 1, 1);
        public static long UnitsPerSecond = (long)Math.Pow(2, 32);
        public static long UnitsPerTick = UnitsPerSecond / TimeSpan.TicksPerSecond;

        public DateTime Contents { get; }
        public char TypeTag { get { return 't'; } }
        public byte[] Bytes { get; }

        public OSCTimeTag(DateTime contents)
        {
            Contents = contents;
            Bytes = new byte[8];

            TimeSpan timespan = contents - BeginningOfTime;
            int seconds = (int)(contents.Ticks / TimeSpan.TicksPerSecond);
            long remainderTicks = contents.Ticks % TimeSpan.TicksPerSecond;
            
            int remainder = (int)(remainderTicks / UnitsPerTick);
            
            Array.Copy(BitConverter.GetBytes(seconds), 0, Bytes, 0, sizeof(int));
            Array.Copy(BitConverter.GetBytes(remainder), 0, Bytes, sizeof(int), sizeof(int));
        }

        public static OSCTimeTag Parse(ArraySegment<byte> bytes)
        {
            MemoryStream stream = new MemoryStream(bytes.Array, bytes.Offset, bytes.Count);
            BinaryReader reader = new BinaryReader(stream);

            int firstPart = reader.ReadInt32();
            int secondPart = reader.ReadInt32();
            
            long remainderTicks = secondPart / UnitsPerTick;
            DateTime time = new DateTime(firstPart * TimeSpan.TicksPerSecond + BeginningOfTime.Ticks + remainderTicks);
            
            return new OSCTimeTag(time);
        }
    }
}
