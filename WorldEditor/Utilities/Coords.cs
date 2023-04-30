using System.Diagnostics.CodeAnalysis;

namespace WorldEditor
{
    public readonly struct Coords
    {
        public int X { get; init; }
        public int Z { get; init; }

        public Coords()
        {
            X = 0;
            Z = 0;
        }
        public Coords(int x, int z)
        {
            X = x;
            Z = z;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Z);
        }
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is not Coords c) return false;

            return X == c.X && Z == c.Z;
        }
    }
}
