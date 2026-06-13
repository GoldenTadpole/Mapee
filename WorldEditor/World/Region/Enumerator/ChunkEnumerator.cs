using CommonUtilities.Factory;

namespace WorldEditor
{
    public class ChunkEnumerator : IChunkEnumerator
    {
        public int RegionsPerTask { get; }
        public Func<IRegionStore> RegionStoreProvider { get; }

        private IChunkEnumeratorFromRegion[] _enumerators;

        public ChunkEnumerator(int regionsPerTask, int tasksPerRegion, Func<IRegionStore> regionStoreProvider, IFactory<int, IChunkEnumeratorFromRegion> factory)
        {
            RegionsPerTask = regionsPerTask;
            RegionStoreProvider = regionStoreProvider;
            _enumerators = new IChunkEnumeratorFromRegion[regionsPerTask];

            for (int i = 0; i < _enumerators.Length; i++)
            {
                _enumerators[i] = factory.Create(tasksPerRegion);
            }
        }

        public virtual void Enumerate(Coords[] regions, IEnumerationBody body)
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

                    ChunkEnumerateFromRegionArgs? args = CreateArgs(regions[index], iterator);
                    if (args is not null)
                    {
                        _enumerators[iterator].Enumerate(args.Value, (r, chunk) => body.EndReadingChunk(iterator, r, chunk));
                    }

                    body.EndReadingRegion(iterator, regions[index]);
                });

                body.EndCycle();
            }
        }

        protected virtual ChunkEnumerateFromRegionArgs? CreateArgs(Coords regionCoords, int iterator)
        {
            IRegionStore regionStore = RegionStoreProvider();

            if (!regionStore.GetData(regionCoords, out byte[]? buffer, out StorageFormat storageFormat)) 
            {
                return null;
            }

            if (buffer is null) 
            {
                return null;
            }

            return new ChunkEnumerateFromRegionArgs(buffer, CreateCoords(regionCoords.X, regionCoords.Z), storageFormat);
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
