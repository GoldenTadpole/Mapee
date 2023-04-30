namespace WorldEditor
{
    public class HardCodedBiomeColor : IBiomeColor
    {
        public BiomeColorType Type { get; } = BiomeColorType.HardCoded;
        public int Color { get; set; }
    }
}
