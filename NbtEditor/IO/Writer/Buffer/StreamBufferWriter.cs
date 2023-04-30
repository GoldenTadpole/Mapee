namespace NbtEditor
{
    public class StreamBufferWriter : BufferWriter
    {
        public Stream OutputStream { get; set; }

        public StreamBufferWriter(Stream outputStream)
        {
            OutputStream = outputStream;
        }

        public override void WriteBuffer(ReadOnlySpan<byte> buffer) 
        {
            OutputStream.Write(buffer);
        }
        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
