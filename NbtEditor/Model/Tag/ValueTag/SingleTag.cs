namespace NbtEditor
{
    public sealed class SingleTag : ValueTag<float> 
    {
        public static implicit operator SingleTag(float value) => new SingleTag(value);
        public static implicit operator float(SingleTag tag) => tag.InternalValue;

        public SingleTag(float value) : base(value, TagId.Single) {}
        public SingleTag() : this(0) { }
    }
}
