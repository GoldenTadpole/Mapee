namespace WorldEditor
{
    public class BlockStateRenamerInstanceConverter : IInstanceConverter<IObject>
    {
        public VersionRange From { get; set; }
        public VersionRange To { get; set; }
        public IBlockRenamer? Renamer { get; set; }

        public IObject? Convert(IObject input, UsageIntent intent)
        {
            if (input is not BlockStateChunk output) return null;
            if (Renamer is null) return output;

            foreach (PaletteSection<Block> section in output.Sections)
            {
                for (int i = 0; i < section.Palette.Length; i++)
                {
                    if (Renamer.Rename(section.Palette[i], out Block renamedBlock))
                    {
                        section.Palette[i] = renamedBlock;
                    }
                }
            }

            return output;
        }
    }
}
