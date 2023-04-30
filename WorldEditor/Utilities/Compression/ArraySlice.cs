namespace WorldEditor
{
    public readonly struct ArraySlice<T>
    {
        public T[] Array { get; }
        public int Position { get; }
        public int Length { get; }

        public ArraySlice(T[] array)
        {
            Array = array;
            Position = 0;
            Length = array.Length;
        }
        public ArraySlice(T[] array, int position, int length)
        {
            Array = array;
            Position = position;
            Length = length;
        }
    }
}
