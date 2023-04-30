using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace Mapper
{
    public readonly struct VecRgb : IEquatable<VecRgb>, ICloneable
    {
        public float R { get; init; }
        public float G { get; init; }
        public float B { get; init; }

        public static VecRgb Empty => -1;
        public static VecRgb Black => 0;
        public static VecRgb White => 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static VecRgb operator +(VecRgb left, VecRgb right)
        {
            return Add(left, right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static VecRgb operator +(VecRgb left, float value)
        {
            return Add(left, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static VecRgb operator +(float value, VecRgb right)
        {
            return Add(right, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static VecRgb operator -(VecRgb left, VecRgb right)
        {
            return Subtract(left, right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static VecRgb operator -(VecRgb left, float value)
        {
            return Subtract(left, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static VecRgb operator -(float value, VecRgb right)
        {
            return Subtract(right, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static VecRgb operator *(VecRgb left, VecRgb right)
        {
            return Multiply(left, right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static VecRgb operator *(VecRgb left, float value)
        {
            return Multiply(left, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static VecRgb operator *(float value, VecRgb right)
        {
            return Multiply(right, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static VecRgb operator /(VecRgb left, VecRgb right)
        {
            return Divide(left, right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static VecRgb operator /(VecRgb left, float value)
        {
            return Divide(left, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static VecRgb operator /(float value, VecRgb right)
        {
            return Divide(right, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator ==(VecRgb left, VecRgb right)
        {
            return Equals(left, right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator !=(VecRgb left, VecRgb right)
        {
            return !Equals(left, right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static implicit operator VecRgb(Color color)
        {
            return new VecRgb(color);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static implicit operator VecRgb(Rgb color)
        {
            return new VecRgb(color);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static implicit operator VecRgb(float value)
        {
            return new VecRgb(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public VecRgb()
        {
            R = 0;
            G = 0;
            B = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public VecRgb(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public VecRgb(Color color)
        {
            R = color.R / 255F;
            G = color.G / 255F;
            B = color.B / 255F;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public VecRgb(Rgb color)
        {
            R = color.R / 255F;
            G = color.G / 255F;
            B = color.B / 255F;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public VecRgb(float value)
        {
            R = value;
            G = value;
            B = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public VecRgb Add(VecRgb value)
        {
            return Add(this, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public VecRgb Add(float value)
        {
            return Add(this, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public VecRgb Subtract(VecRgb value)
        {
            return Subtract(this, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public VecRgb Subtract(float value)
        {
            return Subtract(this, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public VecRgb Multiply(VecRgb value)
        {
            return Multiply(this, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public VecRgb Multiply(float value)
        {
            return Multiply(this, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public VecRgb Divide(VecRgb value)
        {
            return Divide(this, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public VecRgb Divide(float value)
        {
            return Divide(this, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public VecRgb Clamp()
        {
            return Clamp(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public VecRgb Mix(VecRgb other, float intensity)
        {
            return Mix(this, other, intensity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public VecRgb ToBlackAndWhite()
        {
            return ToBlackAndWhite(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public Color ToColor()
        {
            return Color.FromArgb(255, (byte)(R * 255), (byte)(G * 255), (byte)(B * 255));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public Rgb ToByteRgb()
        { 
            return new Rgb((byte)(R * 255), (byte)(G * 255), (byte)(B * 255));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public float GetBrightness()
        {
            return GetBrightness(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public bool IsWhite()
        { 
            return IsWhite(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public bool IsBlack()
        {
            return IsBlack(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public bool IsEmpty()
        {
            return this == Empty;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static VecRgb Add(VecRgb left, VecRgb right)
        {
            return new VecRgb()
            { 
                R = left.R + right.R,
                G = left.G + right.G,
                B = left.B + right.B
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static VecRgb Add(VecRgb left, float value)
        {
            return new VecRgb()
            {
                R = left.R + value,
                G = left.G + value,
                B = left.B + value
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static VecRgb Subtract(VecRgb left, VecRgb right)
        {
            return new VecRgb()
            {
                R = left.R - right.R,
                G = left.G - right.G,
                B = left.B - right.B
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static VecRgb Subtract(VecRgb left, float value)
        {
            return new VecRgb()
            {
                R = left.R - value,
                G = left.G - value,
                B = left.B - value
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static VecRgb Multiply(VecRgb left, VecRgb right)
        {
            return new VecRgb()
            {
                R = left.R * right.R,
                G = left.G * right.G,
                B = left.B * right.B
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static VecRgb Multiply(VecRgb left, float value)
        {
            return new VecRgb()
            {
                R = left.R * value,
                G = left.G * value,
                B = left.B * value
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static VecRgb Divide(VecRgb left, VecRgb right)
        {
            return new VecRgb()
            {
                R = left.R / right.R,
                G = left.G / right.G,
                B = left.B / right.B
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static VecRgb Divide(VecRgb left, float value)
        {
            return new VecRgb()
            {
                R = left.R / value,
                G = left.G / value,
                B = left.B / value
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static VecRgb Clamp(VecRgb rgb)
        {
            return new VecRgb()
            {
                R = rgb.R < 0 ? 0 : rgb.R > 1 ? 1 : rgb.R,
                G = rgb.G < 0 ? 0 : rgb.G > 1 ? 1 : rgb.G,
                B = rgb.B < 0 ? 0 : rgb.B > 1 ? 1 : rgb.B
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static VecRgb Mix(VecRgb main, VecRgb other, float intensity)
        {
            return (other - main) * intensity + main;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static VecRgb ToBlackAndWhite(VecRgb rgb)
        {
            return GetBrightness(rgb);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static float GetBrightness(VecRgb rgb)
        {
            return (rgb.R + rgb.G + rgb.B) / 3;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool IsWhite(VecRgb rgb)
        { 
            return rgb.R >= 1 && rgb.G >= 1 && rgb.B >= 1;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool IsBlack(VecRgb rgb)
        {
            return rgb.R <= 0 && rgb.G <= 0 && rgb.B <= 0;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool Equals(VecRgb left, VecRgb right)
        { 
            return left.R == right.R && left.G == right.G && left.B == right.B;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public object Clone()
        {
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public bool Equals(VecRgb other)
        {
            return Equals(this, other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is VecRgb rgb && Equals(this, rgb);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public override int GetHashCode()
        {
            return HashCode.Combine(R, G, B);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public override string ToString()
        {
            return $"VecRgb [R={R}, G={G}, B={B}]";
        }
    }
}
