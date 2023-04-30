namespace NbtEditor 
{
    public abstract class ValueTag<TValue> : Tag
    {
        public TValue InternalValue { get; set; }

        protected ValueTag(TValue value, TagId id) : base(id)
        {
            InternalValue = value;
        }
    }
}
