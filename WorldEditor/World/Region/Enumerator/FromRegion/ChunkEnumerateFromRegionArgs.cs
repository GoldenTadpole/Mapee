namespace WorldEditor
{
    public readonly struct ChunkEnumerateFromRegionArgs
    {
        public byte[] RegionBuffer { get; }
        public Coords[] ChunksToRead { get; }
        public StorageFormat StorageFormat { get; }

        public ChunkEnumerateFromRegionArgs(byte[] regionBuffer, Coords[] chunksToRead, StorageFormat storageFormat)
        {
            RegionBuffer = regionBuffer;
            ChunksToRead = chunksToRead;
            StorageFormat = storageFormat;
        }
    }
}
