namespace NbtEditor 
{
    public class ValueTagSerializer : ITagSerializer<Tag> 
    {
        public void Serialize(Tag tag, INbtWriter writer) 
        {
            switch (tag.Id)
            {
                case TagId.SignedByte:
                    writer.WriteSignedByte(tag);
                    return;
                case TagId.Int16:
                    writer.WriteInt16(tag);
                    return;
                case TagId.Int32:
                    writer.WriteInt32(tag);
                    return;
                case TagId.Int64:
                    writer.WriteInt64(tag);
                    return;
                case TagId.Single:
                    writer.WriteSingle(tag);
                    return;
                case TagId.Double:
                    writer.WriteDouble(tag);
                    return;
                case TagId.String:
                    writer.WriteString(tag);
                    return;
            }
        }
    }
}
