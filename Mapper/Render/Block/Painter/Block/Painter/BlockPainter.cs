using WorldEditor;

namespace Mapper
{
    public class BlockPainter : IBlockPainter
    {
        public virtual ILightPainter LightPainter { get; set; } = new LightPainter();
        public virtual IElevationPainter ElevationPainter { get; set; } = new ElevationPainter();
        public virtual IStepPainter StepPainter { get; set; } = new StepPainter();

        public virtual RgbA Paint(BlockPainterArgs parameter)
        {
            VecRgb output = PaintIncompleteBlock(parameter).Rgb;
            if (output.IsEmpty()) return RgbA.Empty;

            Step step = parameter.Controller.GetStep(new Coords(parameter.Parameter.X, parameter.Parameter.Z));
            return new RgbA(PaintStep(output, parameter, step));
        }

        protected virtual RgbA PaintIncompleteBlock(BlockPainterArgs parameter)
        {
            RenderBlock renderBlock = parameter.Controller.GetRenderBlock(parameter.Parameter.Block);

            RgbA rgba = renderBlock.BlockColor;
            if (rgba.IsEmpty()) return RgbA.Empty;

            VecRgb output = PaintBiome(rgba.Rgb, renderBlock);

            VecRgb light = LightPainter.FindLight(parameter.Parameter.Block.Data);
            output = LightPainter.PaintBlock(output, light);

            output = ElevationPainter.Paint(output, light, parameter.Parameter.Y, renderBlock.ElevationSettings);

            return new RgbA(output, rgba.A);
        }
        protected virtual VecRgb PaintBiome(VecRgb baseColor, RenderBlock renderBlock)
        {
            VecRgb biomeColor = renderBlock.BiomeColor;
            if (!biomeColor.IsEmpty()) baseColor = (biomeColor * baseColor).Clamp();

            return baseColor;
        }
        protected virtual VecRgb PaintStep(VecRgb baseColor, BlockPainterArgs parameter, Step step)
        {
            return StepPainter.Paint(baseColor, new StepPainterArgs(parameter, step));
        }
    }
}
