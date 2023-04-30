namespace WorldEditor
{
    public readonly struct BlockStatePath
    {
        public string[] Values { get; }
        public string[] Palette { get; }

        public BlockStatePath(string[] values, string[] palette)
        {
            Values = values;
            Palette = palette;
        }
    }
}
