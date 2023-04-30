using CommonUtilities.Pool;

namespace NbtEditor
{
    public class IdTagDeserializer : IIdTagDeserializer<Tag> 
    {
        public IIdTagDeserializer<Tag> ValueTagDeserializer { get; set; }
        public IIdTagDeserializer<ArrayTag> ArrayTagDeserializer { get; set; }
        public ITagDeserializer<ListTag> ListTagDeserializer { get; set; }
        public ITagDeserializer<CompoundTag> CompoundTagDeserializer { get; set; }

        public IdTagDeserializer()
        {
            ValueTagDeserializer = new ValueTagDeserializer(ValueTagAllocation.CreateDefault());
            ArrayTagDeserializer = new ArrayTagDeserializer(ArrayTagAllocation.CreateDefault());
            ListTagDeserializer = new ListTagDeserializer(this, new EmptyPool<int, ListTag>(count => new ListTag(TagId.End, count)));
            CompoundTagDeserializer = new CompoundTagDeserializer(this, new EmptyPool<CompoundTag>(() => new CompoundTag()));
        }
        public IdTagDeserializer(ValueTagAllocation valueTagAllocatio, ArrayTagAllocation arrayTagAllocation, IPool<int, ListTag> listPool, IPool<CompoundTag> compoundPool) 
        {
            ValueTagDeserializer = new ValueTagDeserializer(valueTagAllocatio);
            ArrayTagDeserializer = new ArrayTagDeserializer(arrayTagAllocation);
            ListTagDeserializer = new ListTagDeserializer(this, listPool);
            CompoundTagDeserializer = new CompoundTagDeserializer(this, compoundPool);
        }

        public Tag Deserialize(INbtReader reader, TagId id)
        {
            switch (id)
            {
                case TagId.SignedByte:
                case TagId.Int16:
                case TagId.Int32:
                case TagId.Int64:
                case TagId.Single:
                case TagId.Double:
                case TagId.String:
                    return ValueTagDeserializer.Deserialize(reader, id);
                case TagId.List:
                    return ListTagDeserializer.Deserialize(reader);
                case TagId.Compound:
                    return CompoundTagDeserializer.Deserialize(reader);
                case TagId.SignedByteArray:
                case TagId.Int32Array:
                case TagId.Int64Array:
                    return ArrayTagDeserializer.Deserialize(reader, id);
                default: return null;
            }
        }
    }
}
