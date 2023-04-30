using MapScanner;

namespace Mapper
{
    public readonly struct ChunkRenderArgs
    {
        public IScannedChunk ScannedChunk { get; }
        public IStepProvider StepProvider { get; }

        public ChunkRenderArgs(IScannedChunk chunk, IStepProvider stepProvider)
        {
            ScannedChunk = chunk;
            StepProvider = stepProvider;
        }
    }
}
