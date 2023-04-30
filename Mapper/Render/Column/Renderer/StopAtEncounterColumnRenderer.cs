using MapScanner;

namespace Mapper
{
    public class StopAtEncounterColumnRenderer : IColumnRenderer<ColumnArgs>
    {
        public IBlockPainter BlockPainter { get; set; } = new BlockPainter();

        public VecRgb Render(ColumnArgs input)
        {
            ScannedColumn column = input.Column;

            BlockArgs parameter = new BlockArgs((byte)input.CoordsInChunk.X, column.BottomBlock.FirstInstanceY, (byte)input.CoordsInChunk.Z, column.BottomBlock);
            BlockPainterArgs painterParameter = new BlockPainterArgs(parameter, input.BlockController);

            return BlockPainter.Paint(painterParameter).Rgb;
        }
    }
}
