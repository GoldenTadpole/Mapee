using NbtEditor;

namespace WorldEditor
{
    public class ApiChunk : EmptyChunk, IObject, IDisposable
    {
        public IObject? Biome { get; set; }
        public IObject? BlockState { get; set; }
        public IObject? BlockLight { get; set; }
        public IObject? SkyLight { get; set; }
        public IObject? Heightmap { get; set; }

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

        public void Dispose()
        {
            Biome = null;
            BlockState = null;
            BlockLight = null;
            SkyLight = null;
            Heightmap = null;
        }
    }
}
