namespace NbtEditor
{
    public class ListTagSerializer : ITagSerializer<ListTag>
    {
        public ITagSerializer<Tag> IdTagSerializer { get; set; }

        public ListTagSerializer(ITagSerializer<Tag> idTagSerializer) 
        {
            IdTagSerializer = idTagSerializer;
        }

        public void Serialize(ListTag tag, INbtWriter writer) 
        {
            writer.WriteSignedByte((sbyte)tag.ElementId);
            writer.WriteInt32(tag.Count);

            for (int i = 0; i < tag.Count; i++) { 
                IdTagSerializer.Serialize(tag[i], writer);
            }    
        }
    }
}
