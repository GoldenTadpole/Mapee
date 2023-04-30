using AssetSystem.Biome;
using AssetSystem.Block;
using AssetSystem;
using CommonUtilities.Data;

namespace Mapper
{
    public class AssetComposite<TOutput> where TOutput : struct
    {
        public BiomeAssetArgs Args { get; set; }
        public IDataReader? Data { get; set; }
        public IBiomeAsset<TOutput>? Cloneable { get; set; }
        public IAssetReader<BlockReadArgs, TOutput?>? BlockReader { get; set; }
        public IAssetReader<BiomeReadArgs, TOutput?>? BiomeReader { get; set; }

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
                    if(Data is not null && BlockReader is not null && BiomeReader is not null)
                    {
                        bool secondaryIsNull = false;
                        string secondaryAssetFolder = $"{Args.Block.Source}\\Default";
                        BlockAssetReader<TOutput> blockReader = new(BlockReader);
                        BlockAsset<TOutput>? secondaryAsset = blockReader.Read(new AssetArgs(Data, secondaryAssetFolder));
                        if (secondaryAsset is null)
                        {
                            secondaryAsset = new BlockAsset<TOutput>();
                            secondaryIsNull = true;
                        }

                        string primaryAssetFolder = $"{Args.Block.Source}\\Biome";
                        BiomeAssetReader<TOutput> biomeReader = new(BiomeReader);
                        BiomeAsset<TOutput>? primaryAsset = biomeReader.Read(new AssetArgs(Data, primaryAssetFolder));
                        if (primaryAsset is null)
                        {
                            if (secondaryIsNull) return null;
                            primaryAsset = new BiomeAsset<TOutput>();
                        }

                        output = new CompositeBiomeBlockAsset<TOutput>(primaryAsset, secondaryAsset);
                    }
                }
            }

            output ??= Asset<TOutput>.Empty;
            SetDefaultBiomeValue(output);

            return output;
        }

        private void SetDefaultBiomeValue(IBiomeAsset<TOutput>? output)
        {
            if (output is null) return;

            if (BiomeReader is not null && Args.Block.DefaultOutputExists)
            {
                output.DefaultOutput = BiomeReader.Read(new BiomeReadArgs(null, Args.Block.DefaultOutput, string.Empty)) ?? default;
            }

            if (Args.DefaultBiome is not null && Args.DefaultBiomeExists)
            {
                output.DefaultBiome = Args.DefaultBiome;
            }
        }
    }
}
