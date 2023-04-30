namespace WorldEditor
{
    public class AnvilBlockStateInstanceConverter : IInstanceConverter<IObject>
    {
        public VersionRange From { get; set; } = new(Version.Post_1_1, Version.Snapshot_17w46a);
        public VersionRange To { get; set; } = new(Version.Snapshot_17w47a, Version.Snapshot_18w10a);
        public IIdTranslator? IdTranslator { get; set; }

        public virtual IObject? Convert(IObject input, UsageIntent intent)
        {
            if (input is not AnvilBlockStateChunk oldChunk || IdTranslator is null) return null;

            IBlockStateReader reader = ChunkUtilities.GetBlockStateReader(To.End);
            IBlockStateWriter writer = ChunkUtilities.GetBlockStateWriter(To.End);

            BlockStateChunk output = new();
            foreach (AnvilBlockStateChunk.Section oldSection in oldChunk.Sections)
            {
                PaletteSection<Block>? section = ConvertSection(oldSection, out short[] unlockedArray);
                if (section is null) continue;

                section.Locker = new BlockStateLocker(section)
                {
                    Reader = reader,
                    Writer = writer
                };
                section.Lock(unlockedArray);

                output.Sections.Add(section);
            }

            return output;
        }
        protected virtual PaletteSection<Block>? ConvertSection(AnvilBlockStateChunk.Section section, out short[] unlockedArray)
        {
            unlockedArray = new short[4096];
            if (IdTranslator is null) return null;

            Dictionary<int, short> uniqueBlocks = new(4);
            List<Block> blocks = new(4);

            for (int i = 0; i < 4096; i++)
            {
                byte blockState = section.BlockStates[i];
                byte blockData;
                if (i % 2 == 0)
                {
                    blockData = (byte)(section.BlockData[i / 2] & 0x0F);
                }
                else
                {
                    blockData = (byte)(section.BlockData[i / 2] >> 4);
                }

                int hash = ChunkUtilities.CalculateHashCode(blockState, blockData);
                if (!uniqueBlocks.TryGetValue(hash, out short index))
                {

                    index = (short)uniqueBlocks.Count;
                    uniqueBlocks.Add(hash, index);

                    blocks.Add(IdTranslator.Translate(blockState, blockData));
                }

                unlockedArray[i] = index;
            }

            return new PaletteSection<Block>(blocks.ToArray(), Array.Empty<long>())
            {
                Y = section.Y
            };
        }
    }
}
