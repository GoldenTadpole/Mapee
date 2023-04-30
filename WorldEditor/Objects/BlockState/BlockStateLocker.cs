namespace WorldEditor
{
    public class BlockStateLocker : Locker
    {
        public override int BitCount => Math.Max(4, (int)Math.Ceiling(Math.Log(Section.Palette?.Length ?? 1, 2)));
        public override int UnlockedArrayLength => 16 * 16 * 16;

        protected PaletteSection<Block> Section { get; }

        public BlockStateLocker(PaletteSection<Block> section)
        {
            Section = section;
        }
    }
}
