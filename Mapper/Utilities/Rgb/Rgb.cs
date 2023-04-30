using System.Drawing;

namespace Mapper
{
    public readonly struct Rgb
    {
        public byte R { get; }
        public byte G { get; }
        public byte B { get; }

        public Rgb(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public Color ToColor()
        {
            return Color.FromArgb(R, G, B);
        }
    }
}
