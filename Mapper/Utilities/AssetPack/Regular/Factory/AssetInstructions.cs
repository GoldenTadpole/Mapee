namespace Mapper
{
    public readonly struct AssetInstructions
    {
        public BlockAssetArgs BlockGrouping { get; init; }
        public BlockAssetArgs BlockColor { get; init; }
        public BiomeAssetArgs BiomeColor { get; init; }
        public BiomeAssetArgs Elevation { get; init; }
        public BiomeAssetArgs DepthOpacity { get; init; }
        public BlockAssetArgs Step { get; init; }
        public BlockAssetArgs StepSettings { get; init; }
    }
}
