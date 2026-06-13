namespace WorldEditor
{
    public interface IChunkEnumerator
    {
        void Enumerate(Coords[] regions, IEnumerationBody body);
    }
}
