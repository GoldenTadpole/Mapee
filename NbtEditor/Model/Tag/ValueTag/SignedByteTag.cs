namespace NbtEditor 
{
    public sealed class SignedByteTag : ValueTag<sbyte> 
    {
        public static implicit operator SignedByteTag(sbyte value) => new SignedByteTag(value);
        public static implicit operator sbyte(SignedByteTag tag) => tag.InternalValue;

        public SignedByteTag(sbyte value) : base(value, TagId.SignedByte) {}
        public SignedByteTag() : this(0) { }
    }
}
