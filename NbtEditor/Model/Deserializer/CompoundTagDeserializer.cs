using CommonUtilities.Pool;

namespace NbtEditor
{
    public class CompoundTagDeserializer : ITagDeserializer<CompoundTag>
    {
        public IIdTagDeserializer<Tag> IdTagDeserializer { get; set; }
        public IPool<CompoundTag> PreAllocatedCompoundTags { get; set; }

        public CompoundTagDeserializer(IIdTagDeserializer<Tag> idTagDeserializer, IPool<CompoundTag> preAllocatedCompoundTags)
        {
            IdTagDeserializer = idTagDeserializer;
            PreAllocatedCompoundTags = preAllocatedCompoundTags;
        }

        public CompoundTag Deserialize(INbtReader reader) 
        {
            CompoundTag output = PreAllocatedCompoundTags.Provide();
            output.Clear();

            while (true)
            {
                TagId elementId = (TagId) reader.ReadSignedByte();
                if(elementId == TagId.End) return output;

                string name = reader.ReadString();
                Tag tag = IdTagDeserializer.Deserialize(reader, elementId);
                if (tag is null) continue;

                output.Add(name, tag);
            }
        }
    }
}
