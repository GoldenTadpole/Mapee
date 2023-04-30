namespace NbtEditor
{
    public class ValueTagDeserializer : IIdTagDeserializer<Tag> 
    {
        public ValueTagAllocation ValueTagAllocation { get; set; }

        public ValueTagDeserializer(ValueTagAllocation valueTagAllocation)
        {
            ValueTagAllocation = valueTagAllocation;
        }

        public Tag Deserialize(INbtReader reader, TagId id)
        {
            switch (id) 
            {
                case TagId.SignedByte:
                    return ValueTagAllocation.PreAllocatedSByteTags.Provide(reader.ReadSignedByte());
                case TagId.Int16:
                    return ValueTagAllocation.PreAllocatedInt16Tags.Provide(reader.ReadInt16());
                case TagId.Int32:
                    return ValueTagAllocation.PreAllocatedInt32Tags.Provide(reader.ReadInt32());
                case TagId.Int64:
                    return ValueTagAllocation.PreAllocatedInt64Tags.Provide(reader.ReadInt64());
                case TagId.Single:
                    return ValueTagAllocation.PreAllocatedSingleTags.Provide(reader.ReadSingle());
                case TagId.Double:
                    return ValueTagAllocation.PreAllocatedDoubleTags.Provide(reader.ReadDouble());
                case TagId.String:
                    return ValueTagAllocation.PreAllocatedStringTags.Provide(reader.ReadString());
                default: return null;
            }
        }
    }
}
