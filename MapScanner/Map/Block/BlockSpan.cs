using System;

namespace MapScanner
{
    public readonly struct BlockSpan
    {
        public ScannedBlock Block { get; init; }
        public short Count { get; init; }

        public short TopY => Block.FirstInstanceY;
        public short EndY => (short)(TopY - Count + 1);

        public BlockSpan(ScannedBlock block)
        {
            Block = block;
            Count = 0;
        }
        public BlockSpan(ScannedBlock block, short count)
        {
            Block = block;
            Count = count;
        }

        public BlockSpan Increment(short by = 1)
        {
            return new BlockSpan(Block, (short)(Count + by));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Count, Block);
        }
    }
}
