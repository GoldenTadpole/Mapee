using MapScanner;
using WorldEditor;

namespace Mapper
{
    public readonly struct ColumnArgs
    {
        public ScannedColumn Column { get; init; }
        public Coords CoordsInChunk { get; init; }
        public IBlockController BlockController { get; init; }

        public ColumnArgs(ScannedColumn column, Coords coordsInChunk, IBlockController blockController)
        {
            Column = column;
            CoordsInChunk = coordsInChunk;
            BlockController = blockController;
        }
    }
}
