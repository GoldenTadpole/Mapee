namespace Mapper
{
    public readonly struct BiomeAssetArgs
    {
        public BlockAssetArgs Block { get; init; }
        public string? DefaultBiome { get; init; }
        public bool DefaultBiomeExists { get; init; }

        public BiomeAssetArgs(BlockAssetArgs block, string? defaultBiome) 
        {
            Block = block;
            DefaultBiome = defaultBiome;
        }
    }
}
