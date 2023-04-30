namespace WorldEditor
{
    public class ColormappedBiomeColor : IBiomeColor
    {
        public BiomeColorType Type { get; } = BiomeColorType.Colormapped;
        public float Temperature { get; set; }
        public float Downfall { get; set; }
    }
}
