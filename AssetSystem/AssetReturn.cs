namespace AssetSystem
{
    public readonly struct AssetReturn<TAsset> where TAsset : struct
    {
        public TAsset Asset { get; }
        public bool Exists { get; }

        public static implicit operator TAsset(AssetReturn<TAsset> assetReturn) => assetReturn.Asset;
        public static implicit operator AssetReturn<TAsset>(TAsset asset) => new(asset);

        public AssetReturn(TAsset asset, bool exists = true) 
        {
            Asset = asset;
            Exists = exists;
        }
    }
}
