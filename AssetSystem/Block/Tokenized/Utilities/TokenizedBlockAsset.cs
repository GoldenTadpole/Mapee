namespace AssetSystem.Block
{
    public sealed class TokenizedBlockAsset<TOutput> : ITokenizedBlockAsset<TOutput> where TOutput : struct
    {
        public IDictionary<string, BlockEntry<TOutput>> Blocks { get; set; }
        public TOutput DefaultOutput { get; set; }

        public static TokenizedBlockAsset<TOutput> Empty => new();

        private TokenizedBlockAsset() 
        {
            Blocks = new Dictionary<string, BlockEntry<TOutput>>();
        }

        public AssetReturn<TOutput> Provide(WorldEditor.Block input)
        {
            return new AssetReturn<TOutput>(DefaultOutput, false);
        }

        IAsset<WorldEditor.Block, TOutput> IAsset<WorldEditor.Block, TOutput>.Clone()
        {
            return Clone();
        }
        public ITokenizedBlockAsset<TOutput> Clone()
        {
            ITokenizedBlockAsset<TOutput> output = TokenizedBlockAsset<TOutput>.Empty;
            output.DefaultOutput = DefaultOutput;

            return output;
        }
    }
}
