namespace WorldEditor
{
    public class SimpleBlockStateWriter : IBlockStateWriter
    {
        public long[] Write(short[] unlockedArray, int bitCount)
        {
            int insideCount = 64 / bitCount;
            long[] output = new long[(int)Math.Ceiling(unlockedArray.Length * bitCount / (float)(insideCount * bitCount))];

            int index = 0;
            for (int i = 0; i < output.Length; i++)
            {
                for (int j = 0; j < insideCount; j++)
                {
                    output[i] |= (long)((ulong)unlockedArray[index++] << j * bitCount);

                    if (index >= unlockedArray.Length) return output;
                }
            }

            return output;
        }
    }
}
