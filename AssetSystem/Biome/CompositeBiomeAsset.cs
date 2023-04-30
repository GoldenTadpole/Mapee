namespace AssetSystem.Biome
{
    public class CompositeBiomeAsset<TOutput> : IBiomeAsset<TOutput> where TOutput : struct
    {
        public IBiomeAsset<TOutput> Primary { get; set; }
        public IBiomeAsset<TOutput> Secondary { get; set; }
        public TOutput DefaultOutput { get; set; }

        public string? DefaultBiome
        {
            get => Primary.DefaultBiome;
            set => Primary.DefaultBiome = value;
        }

        public CompositeBiomeAsset(IBiomeAsset<TOutput> primaryAsset, IBiomeAsset<TOutput> secondaryAsset)
        {
            Secondary = secondaryAsset;
            Primary = primaryAsset;
        }

        public AssetReturn<TOutput> Provide(BiomeBlock input)
        {
            AssetReturn<TOutput> primaryReturn = Primary.Provide(input);
            if (primaryReturn.Exists) return primaryReturn;

            AssetReturn<TOutput> secondaryReturn = Secondary.Provide(input);
            if (secondaryReturn.Exists) return secondaryReturn;

            return new AssetReturn<TOutput>(DefaultOutput, false);
        }

        IAsset<BiomeBlock, TOutput> IAsset<BiomeBlock, TOutput>.Clone()
        {
            return Clone();
        }
        public IBiomeAsset<TOutput> Clone()
        {
            return new CompositeBiomeAsset<TOutput>(Primary.Clone(), Secondary.Clone())
            {
                DefaultOutput = DefaultOutput,
                DefaultBiome = Primary.DefaultBiome
            };
        }
    }
}
