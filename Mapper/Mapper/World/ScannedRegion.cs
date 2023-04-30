using CommonUtilities.Collections.Synchronized;
using MapScanner;
using WorldEditor;

namespace Mapper
{
    public class ScannedRegion
    {
        public Coords Coords { get; set; }
        public IList<IScannedChunk> Chunks { get; set; }

        public ScannedRegion(Coords coords)
        {
            Coords = coords;
            Chunks = new SynchronizedList<IScannedChunk>(new List<IScannedChunk>(1024));
        }
    }
}
