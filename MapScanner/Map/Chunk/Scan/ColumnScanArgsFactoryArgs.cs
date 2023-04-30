using WorldEditor;

namespace MapScanner
{
    public readonly struct ColumnScanArgsFactoryArgs
    {
        public ConvertedApiChunk ApiChunk { get; init; }
        public ScannedChunk ScannedChunk { get; init; }
    }
}
