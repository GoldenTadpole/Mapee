namespace WorldEditor
{
    public class NoCompression : IKnownTypeCompression
    {
        public int Compress(ArraySlice<byte> input, ArraySlice<byte> output)
        {
            Array.Copy(input.Array, input.Position, output.Array, output.Position, input.Length);
            return input.Length;
        }
    }
}
