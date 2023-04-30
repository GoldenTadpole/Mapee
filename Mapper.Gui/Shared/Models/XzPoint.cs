using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Mapper.Gui.Model
{
    public struct XzPoint
    {
        public double X { get; set; }
        public double Z { get; set; }

        public double this[int coordinateIndex]
        {
            get
            {
                if (coordinateIndex < 0 || coordinateIndex > 1)
                {
                    throw new IndexOutOfRangeException();
                }

                return coordinateIndex == 0 ? X : Z;
            }
        }

        public static XzPoint Empty => new(double.NaN, double.NaN);

        public static bool operator ==(XzPoint left, XzPoint right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(XzPoint left, XzPoint right)
        {
            return !(left == right);
        }

        public XzPoint(double x, double z)
        {
            X = x;
            Z = z;
        }

        public bool IsEmpty() 
        {
            return double.IsNaN(X) && double.IsNaN(Z);
        }
        public void CastToIntegers()
        {
            X = (int)X;
            Z = (int)Z;
        }

        public static XzPoint FromVector(Vector3 vector)
        {
            return new XzPoint(vector.X, vector.Z);
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is null || obj is not XzPoint other) return false;
            return X == other.X && Z == other.Z;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Z);
        }

        public override string ToString()
        {
            return ToString("0.##");
        }
        public string ToString(string format)
        {
            return $"{((int)Math.Floor(X)).ToString(format)}; {((int)Math.Floor(Z)).ToString(format)}";
        }
    }
}
