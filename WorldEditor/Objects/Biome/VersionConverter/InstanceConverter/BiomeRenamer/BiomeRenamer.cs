namespace WorldEditor
{
    public class BiomeRenamer : IBiomeRenamer
    {
        public IDictionary<string, string> Biomes { get; set; }

        public BiomeRenamer()
        {
            Biomes = new Dictionary<string, string>();
        }

        public string Rename(string biome)
        {
            if (!Biomes.TryGetValue(biome, out string? renamedBiome)) return biome;
            return renamedBiome;
        }
        public static BiomeRenamer FromFile(string file)
        {
            return new BiomeRenamerReader().Read(File.ReadAllText(file));
        }
    }
}
