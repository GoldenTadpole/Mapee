using CommonUtilities.Factory;

namespace WorldEditor
{
    public class ChunkEnumerator : IChunkEnumerator
    {
        public int RegionsPerTask { get; }

        private IChunkEnumeratorFromRegion[] _enumerators;

        public ChunkEnumerator(int regionsPerTask, int tasksPerRegion, IFactory<int, IChunkEnumeratorFromRegion> factory)
        {
            RegionsPerTask = regionsPerTask;
            _enumerators = new IChunkEnumeratorFromRegion[regionsPerTask];

            for (int i = 0; i < _enumerators.Length; i++)
            {
                _enumerators[i] = factory.Create(tasksPerRegion);
            }
        }

        public virtual void Enumerate(string[] regions, IEnumerationBody body)
        {
            int iterations = (int)Math.Ceiling(regions.Length / (float)RegionsPerTask);

            for (int i = 0; i < iterations; i++)
            {
                body.BeginCycle();

                int toExclusiveThis = Math.Min((i + 1) * RegionsPerTask, regions.Length);
                Parallel.For(i * RegionsPerTask, toExclusiveThis, index =>
                {
                    int iterator = index - (i * RegionsPerTask);

                    body.BeginReadingRegion(iterator, regions[index]);

                    ChunkEnumerateFromRegionArgs args = CreateArgs(regions[index], iterator);
                    _enumerators[iterator].Enumerate(args, (r, chunk) => body.EndReadingChunk(iterator, r, chunk));

                    body.EndReadingRegion(iterator, regions[index]);
                });

                body.EndCycle();
            }
        }

        protected virtual ChunkEnumerateFromRegionArgs CreateArgs(string region, int iterator)
        {
            byte[] buffer;
            using (FileStream fileStream = new(region, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                buffer = new byte[fileStream.Length];
                fileStream.Read(buffer);
            }

            Parser.ParseRegionName(Path.GetFileName(region), out int regionX, out int regionZ);
            StorageFormat storageFormat = Parser.ParseStorageFormat(Path.GetFileName(region));

            return new ChunkEnumerateFromRegionArgs(buffer, CreateCoords(regionX, regionZ), storageFormat);
        }
        private static Coords[] CreateCoords(int regionX, int regionZ)
        {
            Coords[] chunksToRead = new Coords[1024];
            for (int i = 0; i < chunksToRead.Length; i++)
            {
                int x = regionX * 32 + i % 32, z = regionZ * 32 + i / 32;
                chunksToRead[i] = new Coords(x, z);
            }

            return chunksToRead;
        }
    }
}
