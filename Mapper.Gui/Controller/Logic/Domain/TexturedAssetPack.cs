namespace Mapper.Gui.Logic
{
    public class TexturedAssetPack
    {
        public Mapper.TexturedAssetPack AssetPack { get; set; }
        public string FilePath { get; set; }

        public TexturedAssetPack(Mapper.TexturedAssetPack assetPack) 
        {
            AssetPack = assetPack;
            FilePath = string.Empty;
        }
    }
}
