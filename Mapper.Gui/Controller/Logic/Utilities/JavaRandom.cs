using System;

namespace Mapper.Gui.Logic
{
    public static class JavaRandom
    {
        public static int NextInt(ulong seed, int n)
        {
            if (n <= 0) throw new ArgumentException("n must be positive");

            seed = TransformSeed(seed);
            if ((n & -n) == n) return (int)((n * Next(31, ref seed)) >> 31);

            long bits, val;
            do
            {
                bits = Next(31, ref seed);
                val = bits % (uint)n;
            } while (bits - val + (n - 1) < 0);

            return (int)val;
        }
        private static uint Next(int bits, ref ulong seed)
        {
            seed = (seed * 0x5DEECE66DL + 0xBL) & ((1L << 48) - 1);

            return (uint)(seed >> (48 - bits));
        }

        private static ulong TransformSeed(ulong seed)
        {
            return (seed ^ 0x5DEECE66DUL) & ((1UL << 48) - 1);
        }
    }
}
