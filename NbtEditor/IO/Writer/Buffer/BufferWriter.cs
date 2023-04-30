namespace NbtEditor
{
    public abstract class BufferWriter : IDisposable
    {
        public abstract void WriteBuffer(ReadOnlySpan<byte> buffer);
        public abstract void Dispose();
    }
}
