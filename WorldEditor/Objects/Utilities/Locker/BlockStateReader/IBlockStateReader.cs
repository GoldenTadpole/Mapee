namespace WorldEditor
{
    public interface IBlockStateReader
    {
        void Read(long[] lockedArray, int bitCount, short[] outputArray);
        short ReadSingle(long[] array, int bitCount, int index);
    }
}
