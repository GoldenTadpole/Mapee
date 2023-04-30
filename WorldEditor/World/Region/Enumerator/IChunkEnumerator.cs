namespace WorldEditor
{
    public interface IChunkEnumerator
    {
        void Enumerate(string[] regions, IEnumerationBody body);
    }
}
