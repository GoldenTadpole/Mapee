namespace NbtEditor
{
    public sealed class StringTag : ValueTag<string> 
    {
        public static implicit operator StringTag(string value) => new StringTag(value);
        public static implicit operator string(StringTag tag) => tag.InternalValue;

        public StringTag(string value) : base(value, TagId.String) {}
        public StringTag() : this(string.Empty) { }
    }
}
