using AssetSystem.Biome;
using AssetSystem;
using AssetSystem.Block;
using MapScanner;
using WorldEditor;

namespace Mapper
{
    public class AssetPack
    {
        public ITokenizedBlockAsset<BlockGrouping> BlockGroupingAsset { get; set; }
        public IAsset<Block, RgbA> BlockColorAsset { get; set; }
        public IBiomeAsset<VecRgb> BiomeColorAsset { get; set; }
        public IBiomeAsset<ElevationSettings> ElevationAsset { get; set; }
        public IBiomeAsset<DepthOpacity> DepthOpacityAsset { get; set; }
        public IAsset<Block, StepType> StepTypeAsset { get; set; }
        public IAsset<Block, StepSettings> StepSettingsAsset { get; set; }

        public AssetPack() 
        {
            BlockGroupingAsset = TokenizedBlockAsset<BlockGrouping>.Empty;
            BlockColorAsset = Asset<Block, RgbA>.Empty;
            BiomeColorAsset = Asset<VecRgb>.Empty;
            ElevationAsset = Asset<ElevationSettings>.Empty;
            DepthOpacityAsset = Asset<DepthOpacity>.Empty;
            StepTypeAsset = Asset<Block, StepType>.Empty;
            StepSettingsAsset = Asset<Block, StepSettings>.Empty;
        }

        public AssetPack Clone() 
        {
            return new AssetPack()
            {
                BlockGroupingAsset = BlockGroupingAsset.Clone(),
                BlockColorAsset = BlockColorAsset.Clone(),
                BiomeColorAsset = BiomeColorAsset.Clone(),
                ElevationAsset = ElevationAsset.Clone(),
                DepthOpacityAsset = DepthOpacityAsset.Clone(),
                StepTypeAsset = StepTypeAsset.Clone(),
                StepSettingsAsset = StepSettingsAsset.Clone()
            };
        }
    }
}
