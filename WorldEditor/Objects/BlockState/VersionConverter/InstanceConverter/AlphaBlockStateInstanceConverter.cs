using System.Security.Cryptography.X509Certificates;

namespace WorldEditor
{
    public class AlphaBlockStateInstanceConverter : IInstanceConverter<IObject>
    {
        public VersionRange From => new(Version.Oldest, Version.Post_Beta_1_3);
        public VersionRange To => new(Version.Post_1_1, Version.Snapshot_17w46a);

        public IObject? Convert(IObject input, UsageIntent intent)
        {
            if (input is not AlphaBlockState blockState) return null;

            AnvilBlockStateChunk output = new();
            for (int i = 0; i < blockState.BlockStates.Length; i += 4096) 
            {
                byte[] blockStates = new byte[4096];
                byte[] blockData = new byte[2048];

                for (int x = 0; x < 16; x++) 
                {
                    for (int z = 0; z < 16; z++) 
                    {
                        for (int y = 0; y < 16; y++) 
                        {
                            int index = x * 2048 + z * 128 + i / 256 + y;
                            int newIndex = y * 256 + z * 16 + x;

                            blockStates[newIndex] = blockState.BlockStates[index];
                            
                            byte data;
                            if (index % 2 == 0) data = (byte)(blockState.BlockData[index / 2] & 0x0F);
                            else data = (byte)(blockState.BlockData[index / 2] >> 4);

                            if (newIndex % 2 == 0) blockData[newIndex / 2] |= data;
                            else blockData[newIndex / 2] |= (byte)(data << 4);
                        }
                    }
                }

                AnvilBlockStateChunk.Section section = new(blockStates, blockData)
                {
                    Y = (sbyte)(i / 4096)
                };

                output.Sections.Add(section);
            }

            return output;
        }
    }
}
