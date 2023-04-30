using MapScanner;
using WorldEditor;

namespace Mapper
{
    public class MapperPack
    {
        public virtual IObjectReader<ChunkParamater, IChunk?>? ChunkReader { get; set; }
        public virtual IVersionConverter? VersionConverter { get; set; }
        public virtual IObjectScanner<ConvertedApiChunk, IScannedChunk>? ChunkScanner { get; set; }
        public virtual IObjectScanner<IScannedChunk, StepChunk>? StepChunkScanner { get; set; }
        public virtual IMapRenderer<MapRenderArgs>? MapRenderer { get; set; }
    }
}
