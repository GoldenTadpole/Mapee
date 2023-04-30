namespace WorldEditor
{
    public class PaletteLocker<TPalette> : Locker
    {
        protected PaletteSection<TPalette> Section { get; set; }

        public override int BitCount => Math.Min(4, (int)Math.Ceiling(Math.Log(Section.Palette.Length, 2)));
        public override int UnlockedArrayLength => 16 * 16 * 16;

        public PaletteLocker(PaletteSection<TPalette> section)
        {
            Section = section;
        }
    }
}
