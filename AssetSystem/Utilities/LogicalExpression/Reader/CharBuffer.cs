namespace AssetSystem
{
    public class Buffer<TValue>
    {
        public TValue[] Values { get; set; }
        public int Index { get; set; }

        public TValue Current => Values[Index - 1];

        public Buffer(TValue[] buffer)
        {
            Values = buffer;
        }

        public bool HasNext()
        {
            return Index < Values.Length;
        }
        public TValue Next()
        {
            return Values[Index++];
        }
    }
}
