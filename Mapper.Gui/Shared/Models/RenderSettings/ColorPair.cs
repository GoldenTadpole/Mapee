using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Media;

namespace Mapper.Gui.Model
{
    public struct ColorPair
    {
        public Color Even { get; set; }
        public Color Odd { get; set; }

        public static ColorPair Overworld => new(Color.FromRgb(10, 10, 10), Color.FromRgb(15, 15, 15));
        public static ColorPair Nether => new(Color.FromRgb(12, 1, 2), Color.FromRgb(20, 2, 3));
        public static ColorPair TheEnd => new(Color.FromRgb(8, 6, 12), Color.FromRgb(15, 10, 20));

        public static bool operator ==(ColorPair left, ColorPair right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(ColorPair left, ColorPair right)
        {
            return !(left == right);
        }

        public ColorPair(Color even, Color odd) 
        {
            Even = even;
            Odd = odd;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if(obj is not ColorPair pair) return false;
            return Even == pair.Even && Odd == pair.Odd;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Even, Odd);
        }
    }
}
