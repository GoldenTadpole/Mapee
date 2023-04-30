using ZLibNet;

namespace WorldEditor
{
    public class ZLibCompression : IKnownTypeCompression
    {
        public int Compress(ArraySlice<byte> input, ArraySlice<byte> output)
        {
            using (MemoryStream inputStream = new MemoryStream(input.Array, input.Position, input.Length))
            using (ZLibStream decompressor = new ZLibStream(inputStream, CompressionMode.Decompress))
            {
                return decompressor.Read(output.Array, output.Position, output.Length);
            }
        }
    }
}
