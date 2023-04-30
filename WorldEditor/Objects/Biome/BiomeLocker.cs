namespace WorldEditor
{
    public class BiomeLocker : Locker
    {
        public override int BitCount => (int)Math.Ceiling(Math.Log(OwnerSection.Palette.Length, 2));
        public override int UnlockedArrayLength => 4 * 4 * 4;

        public PaletteSection<string> OwnerSection { get; protected set; }

        public BiomeLocker(PaletteSection<string> ownerSection)
        {
            OwnerSection = ownerSection;
        }

        public override long[] Lock(short[] unlockedArray)
        {
            if (OwnerSection.Palette.Length < 2) return new long[1];
            return base.Lock(unlockedArray);
        }
        public override void Unlock(long[] lockedArray, short[] outputArray)
        {
            if (lockedArray == null || OwnerSection.Palette == null || OwnerSection.Palette.Length == 1) return;
            base.Unlock(lockedArray, outputArray);
        }
    }
}
