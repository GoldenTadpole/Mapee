namespace NbtEditor
{
    public sealed class Int16Tag : ValueTag<short> 
    {
        public static implicit operator Int16Tag(short value) => new Int16Tag(value);
        public static implicit operator short(Int16Tag tag) => tag.InternalValue;

        public Int16Tag(short value) : base(value, TagId.Int16) {}
        public Int16Tag() : this(0) { }
    }
}
