using AssetSystem.Biome;
using MapScanner;
using WorldEditor;

namespace Mapper
{
    public class RenderBlockSection : IDisposable
    {
        public AssetPack AssetPack { get; set; }
        public Section<Block> BlockSection { get; set; }
        public Section<string>? BiomeSection { get; set; }

        public string DefaultBiome { get; set; } = "minecraft:plains";

        private RenderBlock?[] _renderBlocks;
        private StepSettings[] _stepSettings;
        private DepthOpacity[] _depthOpacity;

        public RenderBlockSection(AssetPack assetPack, Section<Block> blockSection, Section<string>? biomeSection)
        {
            AssetPack = assetPack;
            BlockSection = blockSection;
            BiomeSection = biomeSection;

            _renderBlocks = new RenderBlock?[BlockSection.Palette.Length * (BiomeSection?.Palette?.Length ?? 1)];
            _stepSettings = new StepSettings[BlockSection.Palette.Length];
            _depthOpacity = new DepthOpacity[_renderBlocks.Length];

            Unlock();
        }

        private void Unlock()
        {
            RgbA[] blockColors = new RgbA[BlockSection.Palette.Length];
            for (int i = 0; i < blockColors.Length; i++)
            {
                blockColors[i] = AssetPack.BlockColorAsset.Provide(BlockSection.Palette[i]);
                _stepSettings[i] = AssetPack.StepSettingsAsset.Provide(BlockSection.Palette[i]);
            }

            for (int block = 0; block < BlockSection.Palette.Length; block++)
            {
                int biomeLength = BiomeSection?.Palette?.Length ?? 1;

                for (int biome = 0; biome < biomeLength; biome++)
                {
                    RenderBlock renderBlock = CreateRenderBlock(blockColors[block], block, biome, out DepthOpacity depthOpacity);

                    _renderBlocks[block * biomeLength + biome] = renderBlock;
                    _depthOpacity[block * biomeLength + biome] = depthOpacity;
                }
            }
        }
        private RenderBlock CreateRenderBlock(RgbA blockColor, int blockIndex, int biomeIndex, out DepthOpacity depthOpacity)
        {
            BiomeBlock parameter = new(BlockSection.Palette[blockIndex], BiomeSection?.Palette?[biomeIndex] ?? DefaultBiome);

            VecRgb biomeColor = AssetPack.BiomeColorAsset.Provide(parameter);
            ElevationSettings elevationSettings = AssetPack.ElevationAsset.Provide(parameter);
            depthOpacity = AssetPack.DepthOpacityAsset.Provide(parameter);

            return new RenderBlock(blockColor, biomeColor, elevationSettings);
        }

        public RenderBlock ProvideRenderBlock(BlockData blockData)
        {
            int biomeLength = BiomeSection?.Palette?.Length ?? 1;
            return _renderBlocks?[blockData.IndexInBlockPalette * biomeLength + blockData.IndexInBiomePalette] ?? default;
        }
        public DepthOpacity ProvideDepthOpacity(BlockData blockData)
        {
            int biomeLength = BiomeSection?.Palette?.Length ?? 1;
            return _depthOpacity[blockData.IndexInBlockPalette * biomeLength + blockData.IndexInBiomePalette];
        }
        public StepSettings ProvideStepSettings(BlockData blockData)
        {
            return _stepSettings[blockData.IndexInBlockPalette];
        }

        public void Dispose()
        {
            Array.Clear(_renderBlocks);
            Array.Clear(_stepSettings);
            Array.Clear(_depthOpacity);
        }
    }
}
