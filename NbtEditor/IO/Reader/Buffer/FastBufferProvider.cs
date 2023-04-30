namespace NbtEditor
{
    public class FastBufferProvider : BufferProvider
    {
        public byte[] Buffer { get; set; }
        public int Index { get; set; }

        public FastBufferProvider(byte[] buffer, int index = 0) 
        {
            Buffer = buffer;
            Index = index;
        }

        public override void ProvideBuffer(int count, out byte[] buffer, out int index)
        {
            buffer = Buffer;
            index = Index;

            Index += count;
        }
        public override void Dispose() {
            GC.SuppressFinalize(this);
        }
    }
}
