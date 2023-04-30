namespace WorldEditor
{
    public class SimpleBlockStateReader : IBlockStateReader
    {
        public void Read(long[] array, int bitCount, short[] outputArray)
        {
            int moveBy = 64 - bitCount;
            int outputIndex = 0;

            for (int i = 0; i < array.Length; i++)
            {
                ulong l = (ulong)array[i];

                for (int j = 0; j <= moveBy; j += bitCount)
                {
                    outputArray[outputIndex++] = (short)((l << moveBy - j) >> moveBy);

                    if (outputIndex >= outputArray.Length) return;
                }
            }
        }
        public short ReadSingle(long[] array, int bitCount, int index)
        {
            int fitBits = 64 / bitCount;
            int arrayIndex = index / fitBits;
            ulong l = (ulong)array[arrayIndex];

            int moveBy = 64 - bitCount;
            return (short)(ushort)(l << (moveBy - (index % fitBits * bitCount)) >> moveBy);
        }
    }
}
