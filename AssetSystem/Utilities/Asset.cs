using AssetSystem.Biome;

namespace AssetSystem
{
    public sealed class Asset<TInput, TOutput> : IAsset<TInput, TOutput> where TOutput: struct
    {
        public static Asset<TInput, TOutput> Empty => new();

        public TOutput DefaultOutput { get; set; }

        private Asset() { }

        public AssetReturn<TOutput> Provide(TInput input)
        {
            return new AssetReturn<TOutput>(DefaultOutput, false);
        }
        public IAsset<TInput, TOutput> Clone()
        {
            IAsset<TInput, TOutput> output =  Asset<TInput, TOutput>.Empty;
            output.DefaultOutput = DefaultOutput;

            return output;
        }
    }

    public sealed class Asset<TOutput> : IBiomeAsset<TOutput> where TOutput : struct
    {
        public static Asset<TOutput> Empty => new();

        public string? DefaultBiome { get; set; }
        public TOutput DefaultOutput { get; set; }

        private Asset() { }

        public AssetReturn<TOutput> Provide(BiomeBlock input)
        {
            return new AssetReturn<TOutput>(DefaultOutput, false);
        }

        IAsset<BiomeBlock, TOutput> IAsset<BiomeBlock, TOutput>.Clone()
        {
            return Clone();
        }
        public IBiomeAsset<TOutput> Clone()
        {
            IBiomeAsset<TOutput> output = Asset<TOutput>.Empty;
            output.DefaultOutput = DefaultOutput;
            output.DefaultBiome = DefaultBiome;

            return output;
        }
    }
}
