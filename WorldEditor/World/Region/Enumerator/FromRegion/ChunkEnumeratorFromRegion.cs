using CommonUtilities.Pool;
using NbtEditor;

namespace WorldEditor
{
    public class ChunkEnumeratorFromRegion : IChunkEnumeratorFromRegion
    {
        public virtual int TasksPerRegion { get; } = 8;
        public virtual IObjectReader<ChunkParamater, IChunk?>? ChunkReader { get; set; }
        public virtual ILogger<ChunkError> ErrorLogger { get; set; }
        public virtual ICompression Compression { get; set; }

        public virtual IPool<int, byte[]> DecompressedArrayPool { get; set; }
        public virtual ITagDeserializer TagDeserializer { get; }

        public ChunkEnumeratorFromRegion(int tasksPerRegion = 8)
        {
            TasksPerRegion = tasksPerRegion;

            ChunkReader = new ApiChunkReader();
            ErrorLogger = new ConsoleWriteLogger<ChunkError>();
            Compression = new Compression();

            DecompressedArrayPool = new IndexedObjectPool<byte[]>(TasksPerRegion, i => new byte[1024 * 512]);
            TagDeserializer = new PooledTagDeserializer(TasksPerRegion);
        }

        public virtual void Enumerate(ChunkEnumerateFromRegionArgs args, Action<int, IChunk> body)
        {
            if (args.RegionBuffer.Length < 1) return;

            ParallelUtilities.BufferedFor(0, args.ChunksToRead.Length, TasksPerRegion, (index, r) =>
            {
                try
                {
                    int pos = MathUtilities.NegMod(args.ChunksToRead[index].X, 32) + MathUtilities.NegMod(args.ChunksToRead[index].Z, 32) * 32;
                    int chunkOffset = Parser.ParseInt24(args.RegionBuffer, pos * 4) * 4096;
                    int chunkSize = Parser.ParseInt32(args.RegionBuffer, chunkOffset) - 1;
                    if (chunkOffset == 0 || chunkSize == 0) return;

                    ArraySlice<byte> input = new(args.RegionBuffer, chunkOffset + 5, chunkSize);
                    ArraySlice<byte> output = new(DecompressedArrayPool.Provide(r));
                    CompressionType compressionType = (CompressionType)args.RegionBuffer[chunkOffset + 4];

                    int red = Compression.Compress(input, output, compressionType);
                    if (red < 0) return;

                    CompoundTag? level = TagDeserializer.Deserialize(output, r);
                    if (level is null) return;
                    
                    ChunkParamater chunkParameter = new(level, args.StorageFormat);

                    IChunk? chunk = ChunkReader?.Read(chunkParameter);
                    if (chunk is null) return;

                    chunk.X = args.ChunksToRead[index].X;
                    chunk.Z = args.ChunksToRead[index].Z;
                    chunk.LastModified = Parser.ParseInt32(args.RegionBuffer, pos * 4 + 4096);

                    body?.Invoke(r, chunk);
                }
                catch (Exception e)
                {
                    ErrorLogger.Log(new ChunkError(e, args.ChunksToRead[index]));
                }
            });
        }
    }
}
