namespace NbtEditor 
{
    public sealed class DoubleTag : ValueTag<double> {
        public static implicit operator DoubleTag(double value) => new DoubleTag(value);
        public static implicit operator double(DoubleTag tag) => tag.InternalValue;

        public DoubleTag(double value) : base(value, TagId.Double) {}
        public DoubleTag() : this(0) {}
    }
}
