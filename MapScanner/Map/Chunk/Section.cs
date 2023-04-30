namespace MapScanner
{
    public readonly struct Section<T>
    {
        public sbyte Y { get; }
        public T[] Palette { get; }

        public Section(sbyte y, T[] palette)
        {
            Y = y;
            Palette = palette;
        }
    }
}
