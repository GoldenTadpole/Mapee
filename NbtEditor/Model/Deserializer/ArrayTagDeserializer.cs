namespace NbtEditor
{
    public class ArrayTagDeserializer : IIdTagDeserializer<ArrayTag> 
    {
        public ArrayTagAllocation ArrayTagAllocation { get; set; }

        public ArrayTagDeserializer(ArrayTagAllocation arrayTagAllocation) 
        {
            ArrayTagAllocation = arrayTagAllocation;
        }

        public ArrayTag Deserialize(INbtReader reader, TagId id)
        {
            int length = reader.ReadInt32();

            switch (id)
            {
                case TagId.SignedByteArray:
                    return ArrayTagAllocation.PreAllocatedSByteArrays.Provide(reader.ReadSignedByteArray(length));
                case TagId.Int32Array:
                    return ArrayTagAllocation.PreAllocatedInt32Arrays.Provide(reader.ReadInt32Array(length));
                case TagId.Int64Array:
                    return ArrayTagAllocation.PreAllocatedInt64Arrays.Provide(reader.ReadInt64Array(length));
                default: return null;
            }
        }
    }
}
