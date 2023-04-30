using CommonUtilities.Pool;

namespace NbtEditor {
    public class ValueTagAllocation 
    {
        public virtual IPool<sbyte, SignedByteTag> PreAllocatedSByteTags { get; set; }
        public virtual IPool<short, Int16Tag> PreAllocatedInt16Tags { get; set; }
        public virtual IPool<int, Int32Tag> PreAllocatedInt32Tags { get; set; }
        public virtual IPool<long, Int64Tag> PreAllocatedInt64Tags { get; set; }
        public virtual IPool<float, SingleTag> PreAllocatedSingleTags { get; set; }
        public virtual IPool<double, DoubleTag> PreAllocatedDoubleTags { get; set; }
        public virtual IPool<string, StringTag> PreAllocatedStringTags { get; set; }

        public static ValueTagAllocation CreateDefault() 
        {
            return new ValueTagAllocation()
            {
                PreAllocatedSByteTags = new EmptyPool<sbyte, SignedByteTag>(value => new SignedByteTag(value)),
                PreAllocatedInt16Tags = new EmptyPool<short, Int16Tag>(value => new Int16Tag(value)),
                PreAllocatedInt32Tags = new EmptyPool<int, Int32Tag>(value => new Int32Tag(value)),
                PreAllocatedInt64Tags = new EmptyPool<long, Int64Tag>(value => new Int64Tag(value)),
                PreAllocatedSingleTags = new EmptyPool<float, SingleTag>(value => new SingleTag(value)),
                PreAllocatedDoubleTags = new EmptyPool<double, DoubleTag>(value => new DoubleTag(value)),
                PreAllocatedStringTags = new EmptyPool<string, StringTag>(value => new StringTag(value)),
            };
        }
    }
}
