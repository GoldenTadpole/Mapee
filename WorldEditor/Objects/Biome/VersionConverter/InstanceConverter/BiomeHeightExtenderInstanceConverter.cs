namespace WorldEditor
{
    public class BiomeHeightExtenderInstanceConverter : IInstanceConverter<IObject>
    {
        public VersionRange From { get; set; }
        public VersionRange To { get; set; }
        public int OldHeight { get; set; }
        public int NewHeight { get; set; }

        public IObject? Convert(IObject input, UsageIntent intent)
        {
            if (input is not OldBiome biome) return null;

            short[] array = new short[NewHeight * 4];

            for (int x = 0; x < 4; x++)
            {
                for (int z = 0; z < 4; z++)
                {
                    for (int y = 0; y < NewHeight / 4; y++)
                    {
                        int newIndex = x + y * 16 + z * 4;
                        int oldIndex;

                        if (y < OldHeight / 4)
                        {
                            oldIndex = newIndex;
                        }
                        else
                        {
                            oldIndex = x + (OldHeight - 1) / 4 * 16 + z * 4;
                        }

                        array[newIndex] = biome.Values[oldIndex];
                    }
                }
            }

            biome.Values = array;

            return biome;
        }
    }
}
