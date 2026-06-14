namespace WorldEditor
{
    public class BiomeTimeline
    {
        public IList<(Version Version, string Biome)> Biomes { get; }

        public BiomeTimeline()
        {
            Biomes = new List<(Version Version, string Biome)>();
        }

        public bool Find(Version targetVersion, out string output)
        {
            for (int i = 0; i < Biomes.Count; i++)
            {
                if (targetVersion >= Biomes[i].Version) continue;

                output = i > 0 ? Biomes[i - 1].Biome : Biomes[0].Biome;
                return true;
            }

            if (Biomes.Count > 0)
            {
                output = Biomes[Biomes.Count - 1].Biome;
                return true;
            }

            output = string.Empty;
            return false;
        }
    }
}
