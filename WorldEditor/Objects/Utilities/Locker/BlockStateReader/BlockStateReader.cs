namespace WorldEditor
{
    public class BlockStateReader : IBlockStateReader
    {
        public void Read(long[] lockedArray, int bitCount, short[] outputArray)
        {
            int index = 0, start = 0;

            for (int i = 0; i < lockedArray.Length; i++)
            {
                ulong l = (ulong)lockedArray[i];

                if (start > 0)
                {
                    outputArray[index++] |= (short)(l << (64 - start) >> 64 - bitCount);

                    if (index >= outputArray.Length) return;
                }

                for (int j = start; j <= 64 - bitCount; j += bitCount)
                {
                    outputArray[index++] = (short)(l << (64 - bitCount - j) >> (64 - bitCount));

                    if (index >= outputArray.Length) return;
                }

                int left = (64 - start) % bitCount;
                if (left > 0)
                {
                    outputArray[index] = (short)(l >> (64 - left));
                    start = bitCount - left;
                }
                else start = 0;
            }
        }
        public short ReadSingle(long[] array, int bitCount, int index)
        {
            throw new NotImplementedException();
        }
    }
}
