using AssetSystem;
using MapScanner;
using WorldEditor;

namespace Mapper
{
    public class CachedAssetProvider<TAsset> : IScannedBlockAssetProvider<TAsset> where TAsset : struct
    {
        public TAsset DefaultValue { get; set; }

        private SectionProvider<CachedBlockAssetHolder<TAsset>> _sectionProvider;

        public CachedAssetProvider(IAsset<Block, TAsset> asset, Section<Block>[] sections)
        {
            _sectionProvider = new SectionProvider<CachedBlockAssetHolder<TAsset>>(sections.Length);

            for (int i = 0; i < sections.Length; i++)
            {
                Section<Block> section = sections[i];
                if (section.Palette == null) continue;

                SectionHolder<CachedBlockAssetHolder<TAsset>> holder = new SectionHolder<CachedBlockAssetHolder<TAsset>>(new CachedBlockAssetHolder<TAsset>(asset, section), section.Y);
                _sectionProvider.Sections.Add(holder.Y, holder);
            }
        }

        public TAsset Provide(ScannedBlock block)
        {
            if (!_sectionProvider.TryProvide(block.FirstInstanceY, out CachedBlockAssetHolder<TAsset>? section)) return DefaultValue;
            return section?.Provide(block.Data.IndexInBlockPalette) ?? default;
        }
        public void ResetColumn()
        {
            _sectionProvider.ResetColumn();
        }
    }
}
