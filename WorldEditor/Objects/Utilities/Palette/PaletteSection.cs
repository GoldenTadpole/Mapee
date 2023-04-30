using NbtEditor;

namespace WorldEditor
{
    public class PaletteSection<TPalette> : LockableObject, ISection, IObject
    {
        public sbyte Y { get; set; }
        public Tag? DataTag { get; set; }

        public TPalette[] Palette { get; set; }
        public long[] Indexes { get; set; }

        public SectionType Type
        {
            get
            {
                if (Palette.Length == 1 && (Indexes == null || Indexes.Length < 2)) return SectionType.Uniform;
                return SectionType.Normal;
            }
        }

        protected override long[] LockedArray
        {
            get => Indexes;
            set => Indexes = value;
        }

        public PaletteSection(TPalette[] palette, long[] indexes)
        {
            Palette = palette;
            Indexes = indexes;
            Locker = new PaletteLocker<TPalette>(this);
        }
    }
}
