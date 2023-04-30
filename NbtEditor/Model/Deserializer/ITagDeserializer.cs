namespace NbtEditor
{
    public interface ITagDeserializer<TTag> 
    {
        TTag Deserialize(INbtReader reader);
    }
}
