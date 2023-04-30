namespace Mapper
{
    public readonly struct StepPainterArgs
    {
        public BlockPainterArgs BlockPainterParameter { get; init; }
        public Step Step { get; init; }

        public StepPainterArgs(BlockPainterArgs blockPainterParameter, Step step)
        {
            BlockPainterParameter = blockPainterParameter;
            Step = step;
        }
    }
}
