namespace Mapper
{
    public readonly struct BlockPainterArgs
    {
        public BlockArgs Parameter { get; init; }
        public IBlockController Controller { get; init; }

        public BlockPainterArgs(BlockArgs parameter, IBlockController controller)
        {
            Parameter = parameter;
            Controller = controller;
        }
    }
}
