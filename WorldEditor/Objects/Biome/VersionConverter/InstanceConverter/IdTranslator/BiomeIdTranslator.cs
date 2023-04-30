namespace WorldEditor
{
    public class BiomeIdTranslator : IBiomeIdTranslator
    {
        public IDictionary<int, string> IDs { get; set; }
        public string DefaultBiome { get; set; } = "minecraft:plains";

        public BiomeIdTranslator()
        {
            IDs = new Dictionary<int, string>();
        }

        public string Translate(int id)
        {
            if (!IDs.TryGetValue(id, out string? biome)) return DefaultBiome;
            return biome;
        }
        public static BiomeIdTranslator FromFile(string file)
        {
            return new BiomeIdTranslatorReader().Read(File.ReadAllText(file));
        }
    }
}
