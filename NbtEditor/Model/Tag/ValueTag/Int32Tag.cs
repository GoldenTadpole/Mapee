namespace NbtEditor 
{
    public sealed class Int32Tag : ValueTag<int>
    {
        public static implicit operator Int32Tag(int value) => new Int32Tag(value);
        public static implicit operator int(Int32Tag tag) => tag.InternalValue;

        public Int32Tag(int value) : base(value, TagId.Int32) {}
        public Int32Tag() : this(0) { }
    }
}
