namespace NbtEditor 
{
    public sealed class Int64Tag : ValueTag<long> 
    {
        public static implicit operator Int64Tag(long value) => new Int64Tag(value);
        public static implicit operator long(Int64Tag tag) => tag.InternalValue;

        public Int64Tag(long value) : base(value, TagId.Int64) {}
        public Int64Tag() : this(0) { }
    }
}
