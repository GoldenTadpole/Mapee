namespace NbtEditor
{
    public class ArrayTagSerializer : ITagSerializer<ArrayTag> 
    {
        public void Serialize(ArrayTag tag, INbtWriter writer) 
        {
            writer.WriteInt32(tag.InternalArary.Length);

            switch (tag.Id)
            {
                case TagId.SignedByteArray:
                    writer.WriteSignedByteArray(tag);
                    return;
                case TagId.Int32Array:
                    writer.WriteInt32Array(tag);
                    return;
                case TagId.Int64Array:
                    writer.WriteInt64Array(tag);
                    return;
            }
        }
    }
}
