namespace NbtEditor 
{
    public abstract class BufferProvider : IDisposable
    {
        public abstract void ProvideBuffer(int count, out byte[] buffer, out int index);
        public abstract void Dispose();
    }
}
