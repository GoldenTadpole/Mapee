namespace NbtEditor
{
    public class IdTagSerializer : ITagSerializer<Tag>
    {
        public ITagSerializer<Tag> ValueTagSerializer { get; set; }
        public ITagSerializer<ArrayTag> ArrayTagSerializer { get; set; }
        public ITagSerializer<ListTag> ListTagSerializer { get; set; }
        public ITagSerializer<CompoundTag> CompoundTagSerializer { get; set; }

        public IdTagSerializer()
        {
            ValueTagSerializer = new ValueTagSerializer();
            ArrayTagSerializer = new ArrayTagSerializer();
            ListTagSerializer = new ListTagSerializer(this);
            CompoundTagSerializer = new CompoundTagSerializer(this);
        }

        public void Serialize(Tag tag, INbtWriter writer) 
        {
            switch (tag.Id)
            {
                case TagId.SignedByte:
                case TagId.Int16:
                case TagId.Int32:
                case TagId.Int64:
                case TagId.Single:
                case TagId.Double:
                case TagId.String:
                    ValueTagSerializer.Serialize(tag, writer);
                    return;
                case TagId.List:
                    ListTagSerializer.Serialize((ListTag) tag, writer);
                    return;
                case TagId.Compound:
                    CompoundTagSerializer.Serialize((CompoundTag) tag, writer);
                    return;
                case TagId.SignedByteArray:
                case TagId.Int32Array:
                case TagId.Int64Array:
                    ArrayTagSerializer.Serialize((ArrayTag) tag, writer);
                    return;
            }
        }
    }
}
