using NbtEditor;

namespace WorldEditor
{
    public readonly struct LevelArgs
    {
        public CompoundTag Data { get; }
        public Level Level { get; }

        public LevelArgs(CompoundTag data, Level level) 
        {
            Data = data;
            Level = level;
        }
    }
}