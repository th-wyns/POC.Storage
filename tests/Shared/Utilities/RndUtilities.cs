using System;
using System.Linq;

namespace POC.Storage
{
    static class RndUtilities
    {
        static Random rnd = new Random();

        internal static string NextString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

        internal static int Next(int minValue, int maxValue)
        {
            return rnd.Next(minValue, maxValue);
        }

        internal static int NextInt32()
        {
            int firstBits = rnd.Next(0, 1 << 4) << 28;
            int lastBits = rnd.Next(0, 1 << 28);
            return firstBits | lastBits;
        }

        internal static decimal NextDecimal()
        {
            byte scale = (byte)rnd.Next(29);
            bool sign = rnd.Next(2) == 1;
            return new decimal(NextInt32(),
                               NextInt32(),
                               NextInt32(),
                               sign,
                               scale);
        }

        internal static bool NextBool()
        {
            return rnd.Next(0, 1) == 1 ? true : false;
        }
    }
}
