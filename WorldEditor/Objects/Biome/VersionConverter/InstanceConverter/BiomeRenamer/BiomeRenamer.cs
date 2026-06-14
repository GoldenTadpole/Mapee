namespace WorldEditor
{
    public class BiomeRenamer : IBiomeRenamer
    {
        private readonly HashSet<string> _fastBiomeNameLookup;
        private readonly Dictionary<string, int> _timelineIndices;
        private readonly List<BiomeTimeline> _timelines;

        public BiomeRenamer()
        {
            _fastBiomeNameLookup = new HashSet<string>();
            _timelineIndices = new Dictionary<string, int>();
            _timelines = new List<BiomeTimeline>();
        }

        public string Rename(string biome, Version to)
        {
            if (!_fastBiomeNameLookup.Contains(biome)) return biome;

            if (!_timelineIndices.TryGetValue(biome, out int timelineIndex))
            {
                return biome;
            }

            if (timelineIndex < 0 || timelineIndex >= _timelines.Count) return biome;

            BiomeTimeline timeline = _timelines[timelineIndex];
            if (!timeline.Find(to, out string renamed)) return biome;

            return renamed;
        }

        public void AddTimeline(BiomeTimeline timeline)
        {
            if (timeline.Biomes.Count == 0) return;

            int timelineIndex = _timelines.Count;
            _timelines.Add(timeline);

            for (int i = 0; i < timeline.Biomes.Count - 1; i++)
            {
                string biome = timeline.Biomes[i].Biome;
                _fastBiomeNameLookup.Add(biome);
                _timelineIndices[biome] = timelineIndex;
            }
        }

        public static BiomeRenamer FromFile(string file)
        {
            return new BiomeRenamerReader().Read(File.ReadAllText(file));
        }
    }
}
