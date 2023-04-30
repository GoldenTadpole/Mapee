namespace AssetSystem.Block
{
    public class BlockAsset<TOutput> : ITokenizedBlockAsset<TOutput> where TOutput : struct
    {
        public IDictionary<string, BlockEntry<TOutput>> Blocks { get; set; }
        public TOutput DefaultOutput { get; set; }

        public BlockAsset(int capacity = 0) 
        {
            Blocks = new Dictionary<string, BlockEntry<TOutput>>(capacity);
        }

        public AssetReturn<TOutput> Provide(WorldEditor.Block input)
        {
            if (TryGetOutput(input, out TOutput output)) 
            {
                return output;
            }

            return new AssetReturn<TOutput>(DefaultOutput, false);
        }
        private bool TryGetOutput(WorldEditor.Block input, out TOutput output)
        {
            if (Blocks.TryGetValue(input.Name, out BlockEntry<TOutput>? evaluator))
            {
                if (evaluator.Provide(input.Properties, out output))
                {
                    return true;
                }
            }

            output = default;
            return false;
        }

        IAsset<WorldEditor.Block, TOutput> IAsset<WorldEditor.Block, TOutput>.Clone()
        {
            return Clone();
        }
        public ITokenizedBlockAsset<TOutput> Clone()
        {
            BlockAsset<TOutput> output = new(Blocks.Count) 
            {
                DefaultOutput = DefaultOutput
            };

            foreach (KeyValuePair<string, BlockEntry<TOutput>> pair in Blocks) 
            {
                output.Blocks.Add(pair.Key, (BlockEntry<TOutput>) pair.Value.Clone());
            }

            return output;
        }
    }
}
