namespace WorldEditor
{
    public class BlockStateWriter : IBlockStateWriter
    {
        public long[] Write(short[] unlockedArray, int bitCount)
        {
            long[] output = new long[unlockedArray.Length * bitCount / 64];

            int index = 0, start = 0;
            for (int i = 0; i < output.Length; i++)
            {
                if (start > 0)
                {
                    output[i] = (long)((ulong)unlockedArray[index++] >> bitCount - start);
                }

                for (int j = start; j <= 64 - bitCount; j += bitCount)
                {
                    output[i] |= (long)((ulong)unlockedArray[index++] << j);
                }

                int left = (64 - start) % bitCount;
                if (left > 0)
                {
                    output[i] |= (long)((ulong)unlockedArray[index] << 64 - left);
                    start = bitCount - left;
                }
                else start = 0;
            }

            return output;
        }
    }
}
