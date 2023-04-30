using System;

namespace MapScanner
{
    public readonly struct Int12Pair
    {
        public ushort First => GetFirst();
        public ushort Second => GetSecond();

        public short FirstAsSigned => (short)(GetFirst() + MinSignedUnitValue);
        public short SecondAsSigned => (short)(GetSecond() + MinSignedUnitValue);

        public const int MaxUnitValue = 4095;
        public const int MinSignedUnitValue = -2048;

        public byte B0 { get; } = 0;
        public byte B1 { get; } = 0;
        public byte B2 { get; } = 0;

        public Int12Pair()
        {
            B0 = 0;
            B1 = 0;
            B2 = 0;
        }
        public Int12Pair(ushort first, ushort second)
        {
            B0 = (byte)(first >> 4);
            B1 = (byte)(((first & 0x0F) << 4) | (second & 0x0F));
            B2 = (byte)(second >> 4);
        }
        public Int12Pair(byte b0, byte b1, byte b2)
        {
            B0 = b0;
            B1 = b1;
            B2 = b2;
        }

        private ushort GetFirst()
        {
            return (ushort)((B0 << 4) | (byte)(B1 >> 4));
        }
        private ushort GetSecond()
        {
            return (ushort)((B2 << 4) | (byte)(B1 & 0x0F));
        }

        public static Int12Pair FromSignedInt16(short first, short second)
        {
            return new Int12Pair((ushort)(first + MinSignedUnitValue), (ushort)(second + MinSignedUnitValue));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(B0, B1, B2);
        }
    }
}
