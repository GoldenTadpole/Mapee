using System;
using WorldEditor;

namespace Mapper.Gui.Logic
{
    public static class IntervalMathUtilities
    {
        public static int GetIntervalCount(int leftPoint, int rightPoint, int interval)
        {
            int start = FindIntervalPoint(leftPoint, interval);
            int end = FindIntervalPoint(rightPoint, interval);

            return Math.Abs(end - start) + 1;
        }
        public static int FindIntervalPoint(int point, int mod)
        {
            return (point - NegMod(point, mod)) / mod;
        }
        public static int NegMod(int value, int mod)
        {
            return MathUtilities.NegMod(value, mod);
        }
    }
}
