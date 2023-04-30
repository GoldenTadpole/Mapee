using AssetSystem.Biome;
using AssetSystem;
using CommonUtilities.Data;

namespace Mapper
{
    public sealed class AssetBiome<TOutput> where TOutput : struct
    {
        public BiomeAssetArgs Args { get; set; }
        public IDataReader? Data { get; set; }
        public IBiomeAsset<TOutput>? Cloneable { get; set; }
        public IAssetReader<BiomeReadArgs, TOutput?>? Reader { get; set; }

        private const string USE_DEFAULT_MODIFIER = "*UseDefault";

        public IBiomeAsset<TOutput>? Read()
        {
            IBiomeAsset<TOutput>? output = null;
            if (Args.Block.Source is not null)
            {
                if (Args.Block.Source == USE_DEFAULT_MODIFIER)
                {
                    if (Cloneable is not null) output = Cloneable.Clone();
                }
                else
                {
                    if (Data is not null && Reader is not null) output = new BiomeAssetReader<TOutput>(Reader).Read(new AssetArgs(Data, Args.Block.Source));
                }
            }

            output ??= Asset<TOutput>.Empty;
            SetDefaultBiomeValue(output);

            return output;
        }
        private void SetDefaultBiomeValue(IBiomeAsset<TOutput>? output)
        {
            if (output is null) return;

            if (Reader is not null && Args.Block.DefaultOutputExists)
            {
                output.DefaultOutput = Reader.Read(new BiomeReadArgs(null, Args.Block.DefaultOutput, string.Empty)) ?? default;
            }

            if (Args.DefaultBiome is not null && Args.DefaultBiomeExists)
            {
                output.DefaultBiome = Args.DefaultBiome;
            }
        }
    }
}
