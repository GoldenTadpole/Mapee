using AssetSystem;
using MapScanner;
using WorldEditor;

namespace Mapper
{
    public class CachedBlockAssetHolder<TAsset> where TAsset : struct
    {
        public IAsset<Block, TAsset> Asset { get; set; }
        public Section<Block> Section { get; set; }
        public int Y => Section.Y;

        private TAsset[] _assetCache;
        private bool _unlocked = false;

        public CachedBlockAssetHolder(IAsset<Block, TAsset> asset, Section<Block> section)
        {
            Asset = asset;
            Section = section;

            _assetCache = new TAsset[Section.Palette.Length];
        }

        public TAsset Provide(int index)
        {
            if (!_unlocked)
            {
                for (int i = 0; i < _assetCache.Length; i++)
                {
                    _assetCache[i] = Asset.Provide(Section.Palette[i]);
                }

                _unlocked = true;
            }

            return _assetCache[index];
        }
    }
}
