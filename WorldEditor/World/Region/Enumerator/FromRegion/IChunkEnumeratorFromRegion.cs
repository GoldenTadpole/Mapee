namespace WorldEditor
{
    public interface IChunkEnumeratorFromRegion
    {
        void Enumerate(ChunkEnumerateFromRegionArgs args, Action<int, IChunk> body);
    }
}
