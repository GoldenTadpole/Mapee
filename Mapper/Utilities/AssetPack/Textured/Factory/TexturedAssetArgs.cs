namespace Mapper
{
    public readonly struct TexturedAssetArgs
    {
        public string AssetPath { get; }
        public string TexturePackPath { get; }

        public TexturedAssetArgs(string assetPath, string texturePackPath) 
        {
            AssetPath = assetPath;
            TexturePackPath = texturePackPath;
        }
    }
}
