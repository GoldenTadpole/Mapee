using System;

namespace MapScanner
{
    public readonly struct BlockData
    {
        public ushort IndexInBlockPalette => IndexPairBytes.First;
        public ushort IndexInBiomePalette => IndexPairBytes.Second;
        public byte SkyLight => (byte)(LightByte >> 4);
        public byte BlockLight => (byte)(LightByte & 0x0F);

        public Int12Pair IndexPairBytes { get; }
        public byte LightByte { get; }

        public BlockData(ushort indexInBlockPalette, ushort indexInBiomePalette, byte skyLight, byte blockLight)
        {
            IndexPairBytes = new Int12Pair(indexInBlockPalette, indexInBiomePalette);
            LightByte = (byte)(skyLight << 4 | blockLight);
        }
        public BlockData(Int12Pair indexes, byte light)
        {
            IndexPairBytes = indexes;
            LightByte = light;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(LightByte, IndexPairBytes);
        }
    }
}
