namespace WorldEditor
{
    public class BlockRenamer : IBlockRenamer
    {
        private readonly HashSet<string> _fastBlockNameLookup;
        private readonly Dictionary<string, int> _timelineIndices;
        private readonly List<BlockTimeline> _timelines;

        public BlockRenamer()
        {
            _fastBlockNameLookup = new HashSet<string>();
            _timelineIndices = new Dictionary<string, int>();
            _timelines = new List<BlockTimeline>();
        }

        public bool Rename(Block block, Version to, out Block output)
        {
            output = default;

            if (!_fastBlockNameLookup.Contains(block.Name)) return false;

            Block sortedBlock = SortProperties(block);
            string blockKey = GetBlockKey(sortedBlock);

            if (!_timelineIndices.TryGetValue(blockKey, out int timelineIndex))
            {
                string simpleKey = block.Name;
                if (!_timelineIndices.TryGetValue(simpleKey, out timelineIndex))
                {
                    return false;
                }
            }

            if (timelineIndex < 0 || timelineIndex >= _timelines.Count) return false;

            BlockTimeline timeline = _timelines[timelineIndex];
            if (!timeline.Find(to, out Block replacement)) return false;

            output = CloneBlock(replacement);
            return true;
        }

        public void AddTimeline(BlockTimeline timeline)
        {
            if (timeline.Blocks.Count == 0) return;

            int timelineIndex = _timelines.Count;
            _timelines.Add(timeline);

            foreach ((Version _, Block block) in timeline.Blocks)
            {
                _fastBlockNameLookup.Add(block.Name);

                Block sortedBlock = SortProperties(block);
                string blockKey = GetBlockKey(sortedBlock);
                _timelineIndices[blockKey] = timelineIndex;
            }
        }

        public static BlockRenamer FromFile(string file)
        {
            return new BlockRenamerReader().Read(File.ReadAllText(file));
        }

        private static string GetBlockKey(Block block)
        {
            if (block.Properties.Length == 0) return block.Name;

            Span<char> buffer = stackalloc char[512];
            int pos = 0;

            block.Name.AsSpan().CopyTo(buffer[pos..]);
            pos += block.Name.Length;
            buffer[pos++] = ' ';

            for (int i = 0; i < block.Properties.Length; i++)
            {
                if (i > 0) buffer[pos++] = ';';

                block.Properties[i].Name.AsSpan().CopyTo(buffer[pos..]);
                pos += block.Properties[i].Name.Length;
                buffer[pos++] = '=';
                block.Properties[i].Value.AsSpan().CopyTo(buffer[pos..]);
                pos += block.Properties[i].Value.Length;
            }

            return new string(buffer[..pos]);
        }

        private static Block SortProperties(Block block)
        {
            if (block.Properties.Length <= 1) return block;

            Property[] sorted = new Property[block.Properties.Length];
            block.Properties.CopyTo(sorted, 0);
            Array.Sort(sorted, (a, b) => string.CompareOrdinal(a.Name, b.Name));

            return new Block(block.Name, sorted);
        }

        private static Block CloneBlock(Block block)
        {
            Property[] properties = new Property[block.Properties.Length];
            block.Properties.CopyTo(properties, 0);
            return new Block(block.Name, properties);
        }
    }
}
