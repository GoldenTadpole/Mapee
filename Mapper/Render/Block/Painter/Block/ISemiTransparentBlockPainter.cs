namespace Mapper
{
    public interface ISemiTransparentBlockPainter
    {
        RgbA Paint(BlockPainterArgs parameter);
        RgbA PaintStep(RgbA baseColor, BlockPainterArgs parameter, Step step);
    }
}
