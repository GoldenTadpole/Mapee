namespace WorldEditor
{
    public class Biome2dInstanceConverter : IInstanceConverter<IObject>
    {
        public VersionRange From { get; set; }
        public VersionRange To { get; set; }
        public int Height { get; set; }

        public IObject? Convert(IObject input, UsageIntent intent)
        {
            if (input is not OldBiome biome) return null;

            short[] array = new short[Height * 4];
            for (int z = 0; z < 4; z++)
            {
                for (int x = 0; x < 4; x++)
                {
                    short id = biome.Values[x * 4 + z * 64];

                    for (int y = 0; y < Height / 4; y++)
                    {
                        array[x + y * 16 + z * 4] = id;
                    }
                }
            }

            biome.Values = array;

            return biome;
        }
    }
}
