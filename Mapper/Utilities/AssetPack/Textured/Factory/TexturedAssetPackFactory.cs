using AssetSystem.Biome;
using AssetSystem.Block;
using AssetSystem;
using CommonUtilities.Factory;
using CommonUtilities.Data;
using WorldEditor;

namespace Mapper.Gui.Logic
{
    public class TexturedAssetPackFactory : IFactory<TexturedAssetArgs, TexturedAssetPack>
    {
        public virtual string TexturePath { get; set; } = "assets\\minecraft\\textures";
        public virtual string DefaultBiome { get; set; } = "minecraft:plains";

        public virtual TexturedAssetPack Create(TexturedAssetArgs args)
        {
            IDataReader assetData = new DataReader(args.AssetPath);
            IDataReader textureData = new DataReader(args.TexturePackPath);

            IFactory<string, IReadOnlyBitmap?> bitmapFactory = new DataBitmapFactory(textureData);

            return new TexturedAssetPack()
            {
                BlockColorAsset = CreateBlockColorAsset(assetData, bitmapFactory),
                BiomeColorAsset = CreateBiomeColorAsset(assetData, bitmapFactory),
            };
        }

        protected virtual IAsset<Block, RgbA> CreateBlockColorAsset(IDataReader assetData, IFactory<string, IReadOnlyBitmap?> bitmapFactory)
        {
            BlockAssetReader<RgbA> reader = new(new RgbAPayloadReader(TexturePath, bitmapFactory));
            IAsset<Block, RgbA>? output = reader.Read(new AssetArgs(assetData, "BlockColor"));

            output ??= Asset<Block, RgbA>.Empty;
            output.DefaultOutput = new RgbA(VecRgb.Black, 1);

            return output;
        }
        protected virtual IBiomeAsset<VecRgb> CreateBiomeColorAsset(IDataReader assetData, IFactory<string, IReadOnlyBitmap?> bitmapFactory)
        {
            BiomeAssetReader<VecRgb> reader = new(new VecRgbBiomePayloadReader(TexturePath, bitmapFactory));
            IBiomeAsset<VecRgb>? output = reader.Read(new AssetArgs(assetData, "BiomeColor"));

            output ??= Asset<VecRgb>.Empty;
            output.DefaultBiome = DefaultBiome;
            output.DefaultOutput = VecRgb.Black;

            return output;
        }
    }
}
