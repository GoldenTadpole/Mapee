using System;

namespace MapScanner
{
    public readonly struct ScannedColumn
    {
        public ColumnType Type
        {
            get => BlockSpans.Length > 0 ? ColumnType.SemiTransparent : BottomBlock.IsEmpty() ? ColumnType.Empty : ColumnType.StopAtEncounter;
        }

        public ReadOnlyMemory<BlockSpan> BlockSpans { get; }
        public ScannedBlock BottomBlock { get; }

        public static ScannedColumn Empty => new ScannedColumn(ReadOnlyMemory<BlockSpan>.Empty, new ScannedBlock());

        public ScannedColumn(ScannedBlock block)
        {
            BlockSpans = ReadOnlyMemory<BlockSpan>.Empty;
            BottomBlock = block;
        }
        public ScannedColumn(ReadOnlyMemory<BlockSpan> spans)
        {
            BlockSpans = spans;
            BottomBlock = ScannedBlock.Empty;
        }
        public ScannedColumn(ReadOnlyMemory<BlockSpan> spans, ScannedBlock block)
        {
            BlockSpans = spans;
            BottomBlock = block;
        }

        public override int GetHashCode()
        {
            return GetHashCode(BlockSpans.Span, BottomBlock);
        }
        public static int GetHashCode(ReadOnlySpan<BlockSpan> blockSpans, ScannedBlock bottomBlock)
        {
            int blockSpanHashCode = blockSpans.Length;

            for (int i = 0; i < blockSpans.Length; i++)
            {
                blockSpanHashCode = HashCode.Combine(blockSpanHashCode, blockSpans[i]);
            }

            return HashCode.Combine(blockSpanHashCode, bottomBlock);
        }
    }
}
