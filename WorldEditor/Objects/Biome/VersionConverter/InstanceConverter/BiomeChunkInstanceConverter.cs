namespace WorldEditor
{
    public class BiomeChunkInstanceConverter : IInstanceConverter<IObject>
    {
        public VersionRange From { get; set; } = new(Version.Snapshot_21w06a, Version.experimentalSnapshot_1_18_experimentalSnapshot_7);
        public VersionRange To { get; set; } = new(Version.Snapshot_21w37a, Version.Unknown);
        public IBiomeIdTranslator? Translator { get; set; }

        public IObject? Convert(IObject input, UsageIntent intent)
        {
            if (input is not OldBiome oldBiome || Translator is null) return null;

            IBlockStateReader reader = ChunkUtilities.GetBlockStateReader(To.End);
            IBlockStateWriter writer = ChunkUtilities.GetBlockStateWriter(To.End);

            BiomeChunk output = new();
            int startY = oldBiome.Values.Length == 1536 ? -4 : 0;

            for (int y = 0; y < oldBiome.Values.Length / 64; y++)
            {
                PaletteSection<string>? section = ConvertSection(oldBiome.Values, y, out short[] unlockedArray);
                if (section is null) continue;
                
                section.Y = (sbyte)(y + startY);
                section.Locker = new BiomeLocker(section)
                {
                    Reader = reader,
                    Writer = writer
                };
                section.Lock(unlockedArray);

                output.Sections.Add(section);
            }

            return output;
        }

        protected virtual PaletteSection<string>? ConvertSection(short[] ids, int sectionY, out short[] unlockedArray)
        {
            unlockedArray = new short[64];
            if (Translator is null) return null;

            Dictionary<short, short> uniqueIds = new(4);

            for (int x = 0; x < 4; x++)
            {
                for (int z = 0; z < 4; z++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        short id = ids[x + sectionY * 64 + y * 16 + z * 4];

                        if (!uniqueIds.TryGetValue(id, out short value))
                        {
                            value = (short)uniqueIds.Count;
                            uniqueIds.Add(id, value);
                        }

                        unlockedArray[x + y * 16 + z * 4] = value;
                    }
                }
            }

            PaletteSection<string> output = new(new string[uniqueIds.Count], Array.Empty<long>());
            for (int i = 0; i < uniqueIds.Count; i++)
            {
                output.Palette[i] = Translator.Translate(uniqueIds.ElementAt(i).Key);
            }

            return output;
        }
    }
}
