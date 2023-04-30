namespace WorldEditor
{
    public interface IKnownTypeCompression
    {
        int Compress(ArraySlice<byte> input, ArraySlice<byte> output);
    }
}
