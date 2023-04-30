using NbtEditor;

namespace WorldEditor
{
    public class ConvertedApiChunk : EmptyChunk, IObject, IDisposable
    {
        public BiomeChunk? Biome { get; set; }
        public BlockStateChunk? BlockState { get; set; }
        public LightChunk? BlockLight { get; set; }
        public LightChunk? SkyLight { get; set; }
        public HeightmapCollection? Heightmap { get; set; }

        public Tag? DataTag
        {
            get => Level;
            set
            {
                if (value is null)
                {
                    Level = null;
                    return;
                }

                if (value.GetType() != typeof(CompoundTag)) throw new InvalidCastException();
                Level = value as CompoundTag;
            }
        }

        public ConvertedApiChunk(EmptyChunk chunk) 
        {
            X = chunk.X;
            Z = chunk.Z;
            LastModified = chunk.LastModified;
            Level = chunk.Level;
            Version = chunk.Version;
        }

        public void Dispose()
        {
            Heightmap?.Heightmaps.Clear();
            Biome?.Sections.Clear();
            BlockState?.Sections.Clear();
            SkyLight?.Sections.Clear();
            BlockLight?.Sections.Clear();

            GC.SuppressFinalize(this);
        }
    }
}
