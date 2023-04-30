namespace AssetSystem.Biome
{
    public interface IBiomeAsset<TOutput> : IAsset<BiomeBlock, TOutput> where TOutput : struct
    {
        string? DefaultBiome { get; set; }

        new IBiomeAsset<TOutput> Clone();
    }
}
