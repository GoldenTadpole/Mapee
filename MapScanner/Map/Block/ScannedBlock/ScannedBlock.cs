using System;

namespace MapScanner
{
    public readonly struct ScannedBlock
    {
        public short FirstInstanceY { get; }
        public BlockData Data { get; }

        public static ScannedBlock Empty => new(short.MinValue, new BlockData());

        public ScannedBlock()
        {
            FirstInstanceY = short.MinValue;
            Data = new BlockData();
        }
        public ScannedBlock(short firstInstanceY, BlockData data)
        {
            FirstInstanceY = firstInstanceY;
            Data = data;
        }

        public bool IsEmpty() => FirstInstanceY == short.MinValue;

        public override int GetHashCode()
        {
            return HashCode.Combine(FirstInstanceY, Data);
        }
    }
}
