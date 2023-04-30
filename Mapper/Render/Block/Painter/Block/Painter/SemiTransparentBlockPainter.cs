namespace Mapper
{
    public class SemiTransparentBlockPainter : BlockPainter, ISemiTransparentBlockPainter
    {
        public SemiTransparentBlockPainter()
        {
            StepPainter = new SemiTransparentStepPainter();
        }

        public override RgbA Paint(BlockPainterArgs parameter)
        {
            return base.PaintIncompleteBlock(parameter);
        }
        public virtual RgbA PaintStep(RgbA baseColor, BlockPainterArgs parameter, Step step)
        {
            VecRgb painted = base.PaintStep(baseColor.Rgb, parameter, step);
            return new RgbA(painted, baseColor.A);
        }
    }
}
