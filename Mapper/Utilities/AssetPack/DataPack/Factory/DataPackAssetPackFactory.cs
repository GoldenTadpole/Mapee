using AssetSystem.Biome;
using CommonUtilities.Factory;
using WorldEditor;

namespace Mapper
{
    public class DataPackAssetPackFactory : IFactory<DataPackAssetPackFactoryArgs, DataPackAssetPack?>
    {
        public IObjectReader<DataPackAssetPackFactoryArgs, IBiomeAsset<VecRgb>?> CustomBiomeAssetReader { get; set; }

        public DataPackAssetPackFactory() 
        {
            CustomBiomeAssetReader = new CustomBiomeAssetReader();
        }

        public DataPackAssetPack? Create(DataPackAssetPackFactoryArgs args)
        {
            IBiomeAsset<VecRgb>? asset = CustomBiomeAssetReader.Read(args);
            if (asset is null) return null;

            return new DataPackAssetPack() 
            {
                BiomeColorAsset = asset
            };
        }
    }
}
