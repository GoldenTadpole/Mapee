using CommonUtilities.Pool;

namespace NbtEditor
{
    public class ListTagDeserializer : ITagDeserializer<ListTag>
    {
        public IIdTagDeserializer<Tag> IdTagDeserializer { get; set; }
        public IPool<int, ListTag> PreAllocatedLists { get; set; }

        public ListTagDeserializer(IIdTagDeserializer<Tag> idTagDeserializer, IPool<int, ListTag> preAllocatedLists) 
        {
            IdTagDeserializer = idTagDeserializer;
            PreAllocatedLists = preAllocatedLists;
        }

        public ListTag Deserialize(INbtReader reader) 
        {
            TagId elementId = (TagId)reader.ReadSignedByte();
            int count = reader.ReadInt32();
            if (count < 1) return null;

            ListTag output = PreAllocatedLists.Provide(count);
            output.Clear();
            output.ElementId = elementId;
            
            for (int i = 0; i < count; i++)
            {
                Tag tag = IdTagDeserializer.Deserialize(reader, elementId);
                if (tag is null) continue;

                output.Add(tag);
            }

            return output;
        }
    }
}
