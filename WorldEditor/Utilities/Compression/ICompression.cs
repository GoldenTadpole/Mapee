namespace WorldEditor
{
    public interface ICompression
    {
        int Compress(ArraySlice<byte> input, ArraySlice<byte> output, CompressionType type);
    }
}
