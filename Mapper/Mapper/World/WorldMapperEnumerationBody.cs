using MapScanner;
using System.Collections.Concurrent;
using System.IO;
using WorldEditor;

namespace Mapper
{
    public class WorldMapperEnumerationBody : IEnumerationBody
    {
        public virtual MapperPack MapperPack { get; }
        public virtual Action<Coords, ICanvas>? RegionRendered { get; set; }

        public int RegionsPerTask { get; }
        public int TasksPerRegion { get; }

        private readonly ScannedRegion?[] _savedRegions;
        private readonly ConcurrentBag<ScannedRegion> _finishedRegions;
        private readonly StepProvider _stepProvider;
        private int _count;

        public WorldMapperEnumerationBody(MapperPack mapperPack, int regionsPerTask, int tasksPerRegion)
        {
            MapperPack = mapperPack;
            RegionsPerTask = regionsPerTask;
            TasksPerRegion = tasksPerRegion;

            _savedRegions = new ScannedRegion[RegionsPerTask];
            _finishedRegions = new ConcurrentBag<ScannedRegion>();
            _stepProvider = new StepProvider();
            _count = 0;
        }

        public void ResetScene() 
        {
            for (int i = 0; i < _savedRegions.Length; i++)
            {
                _savedRegions[i] = null;
            }

            _finishedRegions.Clear();
            _stepProvider.Clear();
            _count = 0;
        }

        public void BeginCycle() { }
        public void EndCycle()
        {
            Render(_finishedRegions.ToArray(), _stepProvider);

            for (int i = 0; i < _savedRegions.Length; i++)
            {
                _savedRegions[i] = null;
            }

            _finishedRegions?.Clear();
            _stepProvider.Clear();
        }

        public void BeginReadingRegion(int index, string regionName)
        {
            Parser.ParseRegionName(Path.GetFileName(regionName), out int x, out int z);
            _savedRegions[index] = new ScannedRegion(new Coords(x, z));
        }
        public void EndReadingRegion(int index, string regionName)
        {
            ScannedRegion? region = _savedRegions[index];
            if(region is not null) _finishedRegions.Add(region);

            Interlocked.Increment(ref _count);
            if (_count % 4 == 0) GC.Collect();
        }

        public void EndReadingChunk(int regionIndex, int chunkIndex, IChunk chunk)
        {
            if (chunk is not ApiChunk apiChunk) return;

            ConvertedApiChunk? convertedApiChunk = MapperPack.VersionConverter?.Convert(apiChunk, apiChunk.Version, WorldEditor.Version.Newest, UsageIntent.Read) as ConvertedApiChunk;
            if (convertedApiChunk is null) return;

            Thread.SetData(Thread.GetNamedDataSlot("ThreadIndex"), regionIndex * TasksPerRegion + chunkIndex);
            IScannedChunk? scannedChunk = MapperPack.ChunkScanner?.Scan(convertedApiChunk);

            convertedApiChunk.Dispose();
            if (scannedChunk is null) return;

            StepChunk? stepChunk = MapperPack.StepChunkScanner?.Scan(scannedChunk);
            if(stepChunk is null) return;

            _stepProvider.Add(chunk.X, chunk.Z, stepChunk);
            _savedRegions[regionIndex]?.Chunks.Add(scannedChunk);
        }
        protected virtual void Render(ScannedRegion[] regions, IStepProvider stepProvider)
        {
            if (MapperPack.MapRenderer is null) return;

            Parallel.For(0, regions.Length, i =>
            {
                MapperPack.MapRenderer.Render(new MapRenderArgs(regions[i].Chunks, stepProvider), out ICanvas canvas);
                RegionRendered?.Invoke(regions[i].Coords, canvas);
            });
        }
    }
}
