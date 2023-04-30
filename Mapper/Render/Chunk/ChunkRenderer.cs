using CommonUtilities.Factory;
using WorldEditor;

namespace Mapper
{
    public class ChunkRenderer : IChunkRenderer<ChunkRenderArgs>
    {
        public IColumnRenderer<ColumnArgs> ColumnRenderer { get; set; }
        public IFactory<ChunkRenderArgs, IBlockController> BlockControllerFactory { get; set; }

        public ChunkRenderer(IColumnRenderer<ColumnArgs> columnRenderer, IFactory<ChunkRenderArgs, IBlockController> blockControllerFactory)
        {
            ColumnRenderer = columnRenderer;
            BlockControllerFactory = blockControllerFactory;
        }

        public void Render(ChunkRenderArgs input, ICanvas canvas)
        {
            IBlockController controller = BlockControllerFactory.Create(input);
            for (int i = 0; i < 256; i++)
            {
                ColumnArgs parameter = new ColumnArgs(input.ScannedChunk.GetColumn(i), new Coords(i % 16, i / 16), controller);
                VecRgb color = ColumnRenderer.Render(parameter);

                if (color.IsEmpty()) continue;
                canvas.SetPixel(input.ScannedChunk.Coords.X * 16 + i % 16, input.ScannedChunk.Coords.Z * 16 + i / 16, color.Clamp());
            }

            controller.Dispose();
        }
    }
}
