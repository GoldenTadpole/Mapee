using AssetSystem;
using CommonUtilities.Pool;
using System.Collections.Generic;
using System.Linq;
using WorldEditor;

namespace MapScanner
{
    public class SectionCollection : ISectionCollection
    {
        private IDictionary<int, SectionBlockProviderHolder> Sections { get; set; }
        public int HighestSectionY { get; private set; } = int.MinValue;
        public int LowestSectionY { get; private set; } = int.MaxValue;

        public IResettablePool<short[]> Pool { get; set; }

        private IAsset<Block, BlockGrouping> _asset;

        private class SectionBlockProviderHolder
        {
            public SectionedBlockProvider? Provider { get; set; }
            public SectionGroup SectionGroup { get; set; }
        }

        public SectionCollection(IAsset<Block, BlockGrouping> asset, ConvertedApiChunk apiChunk, IResettablePool<short[]> pool)
        {
            Pool = pool;
            _asset = asset;

            BlockStateChunk? blockStateChunk = apiChunk.BlockState;
            Sections = new Dictionary<int, SectionBlockProviderHolder>(blockStateChunk?.Sections.Count ?? 0);
            if (blockStateChunk is null) return;

            AddSections(apiChunk, blockStateChunk, asset);
        }

        private void AddSections(ConvertedApiChunk apiChunk, BlockStateChunk blockStateChunk, IAsset<Block, BlockGrouping> asset) 
        {
            BiomeChunk? biomeChunk = apiChunk.Biome;
            IDictionary<sbyte, PaletteSection<string>>? biomeSections = biomeChunk?.Sections.ToDictionary(x => x.Y);

            LightChunk? skyLightChunk = apiChunk.SkyLight;
            IDictionary<sbyte, LightChunk.Section>? skyLightSections = skyLightChunk?.Sections.ToDictionary(x => x.Y);

            LightChunk? blockLightChunk = apiChunk.BlockLight;
            IDictionary<sbyte, LightChunk.Section>? blockLightSections = blockLightChunk?.Sections.ToDictionary(x => x.Y);

            for (int i = 0; i < blockStateChunk.Sections.Count; i++)
            {
                PaletteSection<Block> blockSection = blockStateChunk.Sections[i];

                PaletteSection<string>? biomeSection = null;
                biomeSections?.TryGetValue(blockSection.Y, out biomeSection);

                LightChunk.Section? skyLightSection = null;
                skyLightSections?.TryGetValue(blockSection.Y, out skyLightSection);

                LightChunk.Section? blockLightSection = null;
                blockLightSections?.TryGetValue(blockSection.Y, out blockLightSection);

                LightChunk.Section? aboveSkyLightSection = null;
                skyLightSections?.TryGetValue((sbyte)(blockSection.Y + 1), out aboveSkyLightSection);

                LightChunk.Section? aboveBlockLightSection = null;
                blockLightSections?.TryGetValue((sbyte)(blockSection.Y + 1), out aboveBlockLightSection);

                if (blockSection.Y > HighestSectionY) HighestSectionY = blockSection.Y;
                if (blockSection.Y < LowestSectionY) LowestSectionY = blockSection.Y;

                SectionGroup group = new(blockSection)
                {
                    BiomeSection = biomeSection,
                    SkyLightSection = skyLightSection,
                    BlockLightSection = blockLightSection,
                    AboveSkyLightSection = aboveSkyLightSection,
                    AboveBlockLightSection = aboveBlockLightSection
                };

                Sections.Add(blockSection.Y, new SectionBlockProviderHolder()
                {
                    SectionGroup = group
                });
            }

            biomeSections?.Clear();
            skyLightSections?.Clear();
            blockLightSections?.Clear();
        }

        public ISectionedBlockProvider? Provide(int y)
        {
            if (Sections.TryGetValue(y, out SectionBlockProviderHolder? output)) 
            {
                output.Provider ??= new SectionedBlockProvider(_asset, output.SectionGroup, Pool);
                return output.Provider;
            }
            return null;
        }

        public void Dispose()
        {
            Sections.Clear();
            Pool.Reset();
        }
    }
}
