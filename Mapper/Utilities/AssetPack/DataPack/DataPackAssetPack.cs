using AssetSystem.Biome;
using AssetSystem;

namespace Mapper
{
    public class DataPackAssetPack
    {
        public IBiomeAsset<VecRgb> BiomeColorAsset { get; set; }

        public DataPackAssetPack() 
        {
            BiomeColorAsset = Asset<VecRgb>.Empty;
        }
    }
}
