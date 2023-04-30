using MapScanner;

namespace Mapper
{
    public class SemiTransparentColumnRenderer : IColumnRenderer<ColumnArgs>
    {
        public ISemiTransparentBlockPainter BlockPainter { get; set; } = new SemiTransparentBlockPainter();

        public VecRgb Render(ColumnArgs input)
        {
            ScannedColumn column = input.Column;
            Step step = input.BlockController.GetStep(input.CoordsInChunk);

            VecRgb output = VecRgb.Empty;
            float a = 0;

            DepthOpacity lastDepthOpacity = new DepthOpacity();
            float lastDepth = 0;
            ReadOnlySpan<BlockSpan> span = column.BlockSpans.Span;

            for (int i = 0; i < span.Length; i++)
            {
                BlockSpan blockSpan = span[i];
                if (i > 0 && (span[i - 1].EndY - blockSpan.TopY) > 1)
                {
                    lastDepthOpacity = new DepthOpacity();
                    lastDepth = 0;
                }

                BlockArgs parameter = new BlockArgs((byte)input.CoordsInChunk.X, blockSpan.Block.FirstInstanceY, (byte)input.CoordsInChunk.Z, blockSpan.Block);
                BlockPainterArgs painterParameter = new BlockPainterArgs(parameter, input.BlockController);

                DepthOpacity depthOpacity = input.BlockController.GetDepthOpacity(parameter.Block);

                float depth = GetDepth(blockSpan, depthOpacity, lastDepth, lastDepthOpacity);
                if (depth <= 0) continue;

                RgbA color = TryPaintStep(BlockPainter.Paint(painterParameter), painterParameter, step, blockSpan);

                float intensity = GetIntensity(depth, depthOpacity.CalculateOpacity(color.A));
                output = ModifyColorAndOpacity(output, color, intensity, ref a);
                if (a >= 1) return output;

                lastDepthOpacity = depthOpacity;
                lastDepth += depth;
            }

            return PaintBottomBlock(output, a, input, column, step);
        }

        protected virtual float GetDepth(BlockSpan span, DepthOpacity depthOpacity, float lastDepth, DepthOpacity lastDepthOpacity)
        {
            float depth = Math.Min(span.Count, depthOpacity.MaxDepth);
            if (lastDepthOpacity.Equals(depthOpacity) && lastDepth + depth > lastDepthOpacity.MaxDepth)
            {
                depth = lastDepthOpacity.MaxDepth - lastDepth;
            }

            return depth;
        }
        protected virtual RgbA TryPaintStep(RgbA output, BlockPainterArgs painterParameter, Step step, BlockSpan span)
        {
            if (step.BaseY == span.Block.FirstInstanceY)
            {
                output = BlockPainter.PaintStep(output, painterParameter, step);
            }

            return output;
        }
        protected virtual float GetIntensity(float depth, float opacity)
        {
            float intensity = 0;

            float at = depth;
            while (at > 0)
            {
                float whole = 1;
                if (at < 1) whole = at;

                at--;
                intensity += (1 - intensity) * opacity * whole;
            }

            return intensity;
        }
        protected virtual VecRgb ModifyColorAndOpacity(VecRgb output, RgbA color, float intensity, ref float a)
        {
            if (output.IsEmpty())
            {
                output = color.Rgb;
                a = intensity;
            }
            else
            {
                output = output.Mix(color.Rgb, intensity * (1 - a));
                a += (1 - a) * intensity;
            }

            return output;
        }
        protected virtual VecRgb PaintBottomBlock(VecRgb output, float a, ColumnArgs input, ScannedColumn column, Step step)
        {
            if (a < 1 && !column.BottomBlock.IsEmpty())
            {
                BlockArgs parameter = new BlockArgs((byte)input.CoordsInChunk.X, column.BottomBlock.FirstInstanceY, (byte)input.CoordsInChunk.Z, column.BottomBlock);
                BlockPainterArgs painterParameter = new BlockPainterArgs(parameter, input.BlockController);

                RgbA color = BlockPainter.Paint(painterParameter);

                if (step.BaseY == column.BottomBlock.FirstInstanceY) color = BlockPainter.PaintStep(color, painterParameter, step);

                return output.Mix(color.Rgb, 1 - a);
            }

            return output;
        }
    }
}
