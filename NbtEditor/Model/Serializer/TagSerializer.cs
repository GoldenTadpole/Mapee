namespace NbtEditor
{
    public class TagSerializer : ITagSerializer<Tag> {
        public ITagSerializer<Tag> IdTagSerializer { get; set; }

        public TagSerializer() 
        {
            IdTagSerializer = new IdTagSerializer();
        }

        public void Serialize(Tag tag, INbtWriter writer) 
        {
            writer.WriteSignedByte((sbyte) tag.Id);
            writer.WriteString(string.Empty);
            IdTagSerializer.Serialize(tag, writer);
        }
    }
}
