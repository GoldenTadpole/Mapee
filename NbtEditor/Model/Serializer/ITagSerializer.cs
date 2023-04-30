namespace NbtEditor 
{
    public interface ITagSerializer<TTag> 
    {
        void Serialize(TTag tag, INbtWriter writer);
    }
}
