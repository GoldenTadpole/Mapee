namespace WorldEditor
{
    public interface IEnumerationBody
    {
        void BeginCycle();
        void EndCycle();

        void BeginReadingRegion(int index, Coords region);
        void EndReadingRegion(int index, Coords region);

        void EndReadingChunk(int regionIndex, int chunkIndex, IChunk chunk);
    }
}
