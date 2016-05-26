using System;

namespace OSCforPCL.Values
{
    public class OSCTimeTag : IOSCValue<DateTime>
    {
        public static DateTime BeginningOfTime = new DateTime(1900, 1, 1);

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
            long unitsPerSecond = (long)Math.Pow(2, 32);
            long divisor = unitsPerSecond / TimeSpan.TicksPerSecond;
            int remainder = (int)(remainderTicks / divisor);
            
            Array.Copy(BitConverter.GetBytes(seconds), 0, Bytes, 0, sizeof(int));
            Array.Copy(BitConverter.GetBytes(remainder), 0, Bytes, sizeof(int), sizeof(int));
        }
    }
}
