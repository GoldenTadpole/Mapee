namespace WorldEditor
{
    public interface IBlockStateWriter
    {
        long[] Write(short[] unlockedArray, int bitCount);
    }
}
