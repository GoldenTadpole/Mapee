namespace WorldEditor
{
    public static class MathUtilities
    {
        public static int NegMod(int value, int mod)
        {
            int output = value % mod;
            return output < 0 ? output + mod : output;
        }

        public static int FindSectionY(int y, int mod)
        {
            return (y - NegMod(y, mod)) / mod;
        }
        public static int FindSectionY(int y)
        {
            return (y - NegMod(y, 16)) / 16;
        }
        public static int FindBlockIndex(int x, int y, int z)
        {
            return x + NegMod(y, 16) * 256 + z * 16;
        }
        public static int FindBiomeIndex(int x, int y, int z)
        {
            return x / 4 + NegMod(y, 16) / 4 * 16 + z / 4 * 4;
        }
    }
}
