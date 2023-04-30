namespace WorldEditor
{
    public class BiomeRenamerInstanceConverter : IInstanceConverter<IObject>
    {
        public VersionRange From { get; set; }
        public VersionRange To { get; set; }
        public IBiomeRenamer? Renamer { get; set; }

        public IObject? Convert(IObject input, UsageIntent intent)
        {
            if (input is not BiomeChunk output) return null;
            if (Renamer is null) return output;

            IBlockStateReader reader = ChunkUtilities.GetBlockStateReader(To.End);
            IBlockStateWriter? writer = intent == UsageIntent.ReadWrite ? ChunkUtilities.GetBlockStateWriter(To.End) : null;

            foreach (PaletteSection<string> section in output.Sections)
            {
                for (int i = 0; i < section.Palette.Length; i++)
                {
                    section.Palette[i] = Renamer.Rename(section.Palette[i]);
                }

                section.Locker = new BiomeLocker(section)
                {
                    Reader = reader,
                    Writer = writer
                };
            }

            return output;
        }
    }
}
