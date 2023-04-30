namespace NbtEditor 
{
    public class TagDeserializer : ITagDeserializer<Tag>
    {
        public IIdTagDeserializer<Tag> IdTagDeserializer { get; set; }

        public TagDeserializer()
        {
            IdTagDeserializer = new IdTagDeserializer();
        }

        public Tag Deserialize(INbtReader reader)
        {
            TagId id = (TagId) reader.ReadSignedByte();
            reader.ReadString();

            return IdTagDeserializer.Deserialize(reader, id);
        }
    }
}
