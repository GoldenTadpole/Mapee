using System.Buffers;

namespace NbtEditor 
{
    public class StreamBufferProvider : BufferProvider
    {
        public Stream InputStream { get; set; }

        private byte[] _buffer;

        public StreamBufferProvider(Stream inputStream)
        {
            InputStream = inputStream;
        }

        public override void ProvideBuffer(int count, out byte[] buffer, out int index) 
        {
            ReturnBuffer();
            _buffer = ArrayPool<byte>.Shared.Rent(count);

            buffer = _buffer;
            index = 0;

            InputStream.Read(buffer, 0, count);
        }
        public override void Dispose()
        {
            ReturnBuffer();
            GC.SuppressFinalize(this);
        }

        private void ReturnBuffer()
        {
            if (_buffer is not null) ArrayPool<byte>.Shared.Return(_buffer);
        }
    }
}
