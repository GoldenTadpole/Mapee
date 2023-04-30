namespace WorldEditor
{
    public class EmptyChunkReader : IObjectReader<ChunkParamater, EmptyChunk>
    {
        public virtual IChunkVersionFinder VersionFinder { get; set; }

        public EmptyChunkReader()
        {
            VersionFinder = new ChunkVersionFinder();
        }

        public EmptyChunk Read(ChunkParamater input)
        {
            EmptyChunk output = new EmptyChunk()
            {
                Level = input.Level,
                Version = VersionFinder.FindVersion(input)
            };
            ChunkUtilities.ReadCoordinates(input.Level, output.Version, output);

            return output;
        }
    }
}
