using AssetSystem.Biome;
using AssetSystem;
using WorldEditor;

namespace Mapper
{
    public class TexturedAssetPack
    {
        public IAsset<Block, RgbA> BlockColorAsset { get; set; }
        public IBiomeAsset<VecRgb> BiomeColorAsset { get; set; }
        public Colormap Colormap { get; set; }

        public TexturedAssetPack() 
        {
            BlockColorAsset = Asset<Block, RgbA>.Empty;
            BiomeColorAsset = Asset<VecRgb>.Empty;
        }
    }
}
