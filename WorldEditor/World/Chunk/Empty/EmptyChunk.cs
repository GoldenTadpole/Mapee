using NbtEditor;

namespace WorldEditor
{
    public class EmptyChunk : IChunk
    {
        public int X { get; set; }
        public int Z { get; set; }
        public int LastModified { get; set; }
        public CompoundTag? Level { get; set; }

        public Version Version { get; set; }
    }
}
