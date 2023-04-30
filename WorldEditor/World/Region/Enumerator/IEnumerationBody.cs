namespace WorldEditor
{
    public interface IEnumerationBody
    {
        void BeginCycle();
        void EndCycle();

        void BeginReadingRegion(int index, string regionName);
        void EndReadingRegion(int index, string regionName);

        void EndReadingChunk(int regionIndex, int chunkIndex, IChunk chunk);
    }
}
