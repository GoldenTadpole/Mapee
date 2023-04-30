namespace WorldEditor
{
    public class Compression : ICompression
    {
        public IKnownTypeCompression ZLibCompression { get; set; } = new ZLibCompression();
        public IKnownTypeCompression GZipCompression { get; set; } = new GZipCompression();
        public IKnownTypeCompression NoCompression { get; set; } = new NoCompression();

        public int Compress(ArraySlice<byte> input, ArraySlice<byte> output, CompressionType type)
        {
            switch (type)
            {
                case CompressionType.ZLib:
                    return ZLibCompression.Compress(input, output);
                case CompressionType.GZip:
                    return GZipCompression.Compress(input, output);
                case CompressionType.Uncompressed:
                    return NoCompression.Compress(input, output);
                default: return -1;
            }
        }
    }
}
