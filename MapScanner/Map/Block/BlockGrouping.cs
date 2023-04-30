using System;

namespace MapScanner
{
    public readonly struct BlockGrouping : IEquatable<BlockGrouping>
    {
        public BlockType Type { get; init; }
        public short ExitOnDepth { get; init; }

        private readonly int _identifier;

        public static BlockGrouping Empty => new BlockGrouping();

        public BlockGrouping()
        {
            Type = BlockType.Disabled;
            ExitOnDepth = short.MaxValue;
            _identifier = int.MinValue;
        }
        public BlockGrouping(BlockType blockType)
        {
            Type = blockType;
            ExitOnDepth = short.MaxValue;
            _identifier = int.MinValue;
        }
        public BlockGrouping(BlockType blockType, int identifier)
        {
            Type = blockType;
            ExitOnDepth = short.MaxValue;
            _identifier = identifier;
        }

        public bool Equals(BlockGrouping other)
        {
            if (_identifier == int.MinValue || other._identifier == int.MinValue) return false;
            return _identifier == other._identifier;
        }
    }
}
