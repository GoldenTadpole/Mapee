namespace WorldEditor
{
    public class LightInstanceConverter : IInstanceConverter<LightChunk?>
    {
        public VersionRange From => new(Version.Oldest, Version.Post_Beta_1_3);
        public VersionRange To => new(Version.Post_1_1, Version.Newest);

        public LightChunk? Convert(IObject input, UsageIntent intent)
        {
            if (input is not OldLight oldLight) return null;

            LightChunk output = new();
            for (int i = 0; i < oldLight.Values.Length; i += 2048)
            {
                byte[] values = new byte[2048];
                for (int x = 0; x < 16; x++)
                {
                    for (int z = 0; z < 16; z++)
                    {
                        for (int y = 0; y < 16; y++)
                        {
                            int index = x * 2048 + z * 128 + i / 128 + y;
                            int newIndex = y * 256 + z * 16 + x;

                            byte light;
                            if (index % 2 == 0) light = (byte)(oldLight.Values[index / 2] & 0x0F);
                            else light = (byte)(oldLight.Values[index / 2] >> 4);

                            if (newIndex % 2 == 0) values[newIndex / 2] |= light;
                            else values[newIndex / 2] |= (byte)(light << 4);
                        }
                    }
                }

                LightChunk.Section section = new(values)
                {
                    Y = (sbyte)(i / 2048),
                };

                output.Sections.Add(section);
            }

            return output;
        }
    }
}
