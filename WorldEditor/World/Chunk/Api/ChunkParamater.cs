using NbtEditor;

namespace WorldEditor
{
    public readonly struct ChunkParamater
    {
        public CompoundTag Level { get; init; }
        public StorageFormat StorageFormat { get; init; }

        public ChunkParamater(CompoundTag level, StorageFormat storageFormat)
        {
            Level = level;
            StorageFormat = storageFormat;
        }
    }
}
