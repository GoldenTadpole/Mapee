using WorldEditor;

namespace MapScanner
{
    public struct SectionGroup
    {
        public PaletteSection<Block> BlockSection { get; init; }
        public PaletteSection<string>? BiomeSection { get; init; }
        public LightChunk.Section? SkyLightSection { get; init; }
        public LightChunk.Section? BlockLightSection { get; init; }

        public LightChunk.Section? AboveSkyLightSection { get; init; }
        public LightChunk.Section? AboveBlockLightSection { get; init; }

        public SectionGroup(PaletteSection<Block> blockSection)
        {
            BlockSection = blockSection;
        }
    }
}
