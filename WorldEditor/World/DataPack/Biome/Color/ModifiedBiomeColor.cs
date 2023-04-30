namespace WorldEditor
{
    public class ModifiedBiomeColor : IBiomeColor
    {
        public BiomeColorType Type { get; } = BiomeColorType.Modified;
        public string? Modifier { get; set; }
    }
}
