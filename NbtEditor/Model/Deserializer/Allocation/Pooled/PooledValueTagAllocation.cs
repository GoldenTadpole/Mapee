using CommonUtilities.Pool;

namespace NbtEditor
{
    public class PooledValueTagAllocation : ValueTagAllocation
    {
        private ExpandableObjectPool<sbyte, SignedByteTag> _signedByte;
        private ExpandableObjectPool<short, Int16Tag> _int16;
        private ExpandableObjectPool<int, Int32Tag> _int32;
        private ExpandableObjectPool<long, Int64Tag> _int64;
        private ExpandableObjectPool<float, SingleTag> _single;
        private ExpandableObjectPool<double, DoubleTag> _double;
        private ExpandableObjectPool<string, StringTag> _string;

        public PooledValueTagAllocation()
        {
            _signedByte = new ExpandableObjectPool<sbyte, SignedByteTag>(0, value => new SignedByteTag(value));
            PreAllocatedSByteTags = new TagPool<sbyte, SignedByteTag>(_signedByte,
                (value, tag) =>
                {
                    tag.InternalValue = value;
                    return tag;
                });

            _int16 = new ExpandableObjectPool<short, Int16Tag>(0, value => new Int16Tag(value));
            PreAllocatedInt16Tags = new TagPool<short, Int16Tag>(_int16,
            (value, tag) =>
            {
                tag.InternalValue = value;
                return tag;
            });

            _int32 = new ExpandableObjectPool<int, Int32Tag>(0, value => new Int32Tag(value));
            PreAllocatedInt32Tags = new TagPool<int, Int32Tag>(_int32,
            (value, tag) =>
            {
                tag.InternalValue = value;
                return tag;
            });

            _int64 = new ExpandableObjectPool<long, Int64Tag>(0, value => new Int64Tag(value));
            PreAllocatedInt64Tags = new TagPool<long, Int64Tag>(_int64,
            (value, tag) =>
            {
                tag.InternalValue = value;
                return tag;
            });

            _single = new ExpandableObjectPool<float, SingleTag>(0, value => new SingleTag(value));
            PreAllocatedSingleTags = new TagPool<float, SingleTag>(_single,
            (value, tag) =>
            {
                tag.InternalValue = value;
                return tag;
            });

            _double = new ExpandableObjectPool<double, DoubleTag>(0, value => new DoubleTag(value));
            PreAllocatedDoubleTags = new TagPool<double, DoubleTag>(_double,
            (value, tag) =>
            {
                tag.InternalValue = value;
                return tag;
            });

            _string = new ExpandableObjectPool<string, StringTag>(0, value => new StringTag(value));
            PreAllocatedStringTags = new TagPool<string, StringTag>(_string,
            (value, tag) =>
            {
                tag.InternalValue = value;
                return tag;
            });
        }

        public void Reset()
        {
            _signedByte.Reset();
            _int16.Reset();
            _int32.Reset();
            _int64.Reset();
            _single.Reset();
            _double.Reset();
            _string.Reset();
        }
    }
}
