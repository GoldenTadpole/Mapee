namespace NbtEditor
{
    public interface IIdTagDeserializer<TTag>
    {
        TTag Deserialize(INbtReader reader, TagId id);
    }
}
