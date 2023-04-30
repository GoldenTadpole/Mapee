using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace Mapper.Gui.Model
{
    public struct XzRange
    {
        public XzPoint TopLeftPoint { get; set; }
        public XzPoint BottomRightPoint { get; set; }

        public XzPoint Size => new(Math.Abs(TopLeftPoint.X - BottomRightPoint.X - 1), Math.Abs(TopLeftPoint.Z - BottomRightPoint.Z - 1));

        public static XzRange Empty => new(XzPoint.Empty, XzPoint.Empty);

        public static bool operator ==(XzRange left, XzRange right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(XzRange left, XzRange right)
        {
            return !(left == right);
        }

        public XzRange(XzPoint topLeftPoint, XzPoint bottomRightPoint)
        {
            TopLeftPoint = topLeftPoint;
            BottomRightPoint = bottomRightPoint;
        }
        public XzRange(double topLeftX, double topLeftZ, double bottomRightX, double bottomRightZ)
        {
            TopLeftPoint = new XzPoint(topLeftX, topLeftZ);
            BottomRightPoint = new XzPoint(bottomRightX, bottomRightZ);
        }

        public bool IsEmpty() 
        {
            return TopLeftPoint.IsEmpty() && BottomRightPoint.IsEmpty();
        }
        public bool IsWithinBoundsIgnoreCorner(int x, int z)
        {
            return x >= 0 && x < (int)Size.X && z >= 0 && z < (int)Size.Z;
        }
        public Rect ToRectangle()
        {
            return new Rect(TopLeftPoint.X, TopLeftPoint.Z, BottomRightPoint.X, BottomRightPoint.Z);
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is not XzRange range) return false;
            return TopLeftPoint == range.TopLeftPoint && BottomRightPoint == range.BottomRightPoint;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(TopLeftPoint, BottomRightPoint);
        }
    }
}
