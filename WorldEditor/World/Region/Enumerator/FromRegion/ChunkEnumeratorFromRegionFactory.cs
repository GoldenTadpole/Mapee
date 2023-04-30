using CommonUtilities.Factory;

namespace WorldEditor
{
    public class ChunkEnumeratorFromRegionFactory : IFactory<int, IChunkEnumeratorFromRegion>
    {
        public Func<int, IObjectReader<ChunkParamater, IChunk?>?> ChunkReaderCreator { get; set; }

        public ChunkEnumeratorFromRegionFactory(Func<int, IObjectReader<ChunkParamater, IChunk?>?> chunkReaderCreator)
        {
            ChunkReaderCreator = chunkReaderCreator;
        }

        public IChunkEnumeratorFromRegion Create(int index)
        {
            return new ChunkEnumeratorFromRegion(index)
            {
                ChunkReader = ChunkReaderCreator(index)
            };
        }
    }
}
