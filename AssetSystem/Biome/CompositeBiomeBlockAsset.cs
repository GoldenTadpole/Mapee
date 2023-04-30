namespace AssetSystem.Biome
{
    public class CompositeBiomeBlockAsset<TOutput> : IBiomeAsset<TOutput> where TOutput: struct
    {
        public IBiomeAsset<TOutput> Primary { get; set; }
        public IAsset<WorldEditor.Block, TOutput> Secondary { get; set; }
        public TOutput DefaultOutput { get; set; }

        public string? DefaultBiome
        {
            get => Primary.DefaultBiome;
            set => Primary.DefaultBiome = value;
        }

        public CompositeBiomeBlockAsset(IBiomeAsset<TOutput> primaryAsset, IAsset<WorldEditor.Block, TOutput> secondaryAsset)
        {
            Secondary = secondaryAsset;
            Primary = primaryAsset;
        }

        public AssetReturn<TOutput> Provide(BiomeBlock input)
        {
            AssetReturn<TOutput> primaryReturn = Primary.Provide(input);
            if (primaryReturn.Exists) return primaryReturn;

            AssetReturn<TOutput> secondaryReturn = Secondary.Provide(input.Block);
            if (secondaryReturn.Exists) return secondaryReturn;

            return new AssetReturn<TOutput>(DefaultOutput, false);
        }

        IAsset<BiomeBlock, TOutput> IAsset<BiomeBlock, TOutput>.Clone()
        {
            return Clone();
        }
        public IBiomeAsset<TOutput> Clone()
        {
            return new CompositeBiomeBlockAsset<TOutput>(Primary.Clone(), Secondary.Clone()) 
            {
                DefaultOutput = DefaultOutput,
                DefaultBiome = Primary.DefaultBiome
            };
        }
    }
}
