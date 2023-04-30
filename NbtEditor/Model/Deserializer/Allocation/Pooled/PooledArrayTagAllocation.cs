using CommonUtilities.Pool;

namespace NbtEditor
{
    public class PooledArrayTagAllocation : ArrayTagAllocation
    {
        private ExpandableObjectPool<sbyte[], ArrayTag> _sbyteArray;
        private ExpandableObjectPool<int[], ArrayTag> _int32Array;
        private ExpandableObjectPool<long[], ArrayTag> _int64Array;

        public PooledArrayTagAllocation()
        {
            _sbyteArray = new ExpandableObjectPool<sbyte[], ArrayTag>(0, array => new ArrayTag(array));
            PreAllocatedSByteArrays = new TagPool<sbyte[], ArrayTag>(_sbyteArray,
                (array, tag) =>
                {
                    tag.InternalArary = array;
                    tag.ChangeId(TagId.SignedByteArray);
                    return tag;
                });

            _int32Array = new ExpandableObjectPool<int[], ArrayTag>(0, (array) => new ArrayTag(array));
            PreAllocatedInt32Arrays = new TagPool<int[], ArrayTag>(_int32Array,
            (array, tag) =>
            {
                tag.InternalArary = array;
                tag.ChangeId(TagId.Int32Array);
                return tag;
            });

            _int64Array = new ExpandableObjectPool<long[], ArrayTag>(0, (array) => new ArrayTag(array));
            PreAllocatedInt64Arrays = new TagPool<long[], ArrayTag>(_int64Array,
            (array, tag) =>
            {
                tag.InternalArary = array;
                tag.ChangeId(TagId.Int64Array);
                return tag;
            });
        }

        public void Reset()
        {
            _sbyteArray.Reset();
            _int32Array.Reset();
            _int64Array.Reset();
        }
    }
}
