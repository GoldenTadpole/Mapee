namespace AssetSystem.Block
{
    public interface ITokenizedBlockAsset<TOutput> : IAsset<WorldEditor.Block, TOutput> where TOutput : struct
    {
        IDictionary<string, BlockEntry<TOutput>> Blocks { get; set; }

        new ITokenizedBlockAsset<TOutput> Clone();
    }
}
