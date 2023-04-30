namespace Mapper
{
    public readonly struct RenderBlock
    {
        public RgbA BlockColor { get; }
        public VecRgb BiomeColor { get; }
        public ElevationSettings ElevationSettings { get; }

        public RenderBlock(RgbA blockColor, VecRgb biomeColor, ElevationSettings elevationSettings)
        {
            BlockColor = blockColor;
            BiomeColor = biomeColor;
            ElevationSettings = elevationSettings;
        }
    }
}
