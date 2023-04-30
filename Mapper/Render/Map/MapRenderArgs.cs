using MapScanner;

namespace Mapper
{
    public readonly struct MapRenderArgs
    {
        public IList<IScannedChunk> Chunks { get; }
        public IStepProvider StepProvider { get; }

        public MapRenderArgs(IList<IScannedChunk> chunks, IStepProvider stepProvider)
        {
            Chunks = chunks;
            StepProvider = stepProvider;
        }
    }
}
