using MapScanner;

namespace Mapper
{
    public readonly struct BlockArgs
    {
        public byte X { get; init; }
        public short Y { get; init; }
        public byte Z { get; init; }
        public ScannedBlock Block { get; init; }

        public BlockArgs(byte x, short y, byte z, ScannedBlock block)
        {
            X = x;
            Y = y;
            Z = z;
            Block = block;
        }
    }
}
