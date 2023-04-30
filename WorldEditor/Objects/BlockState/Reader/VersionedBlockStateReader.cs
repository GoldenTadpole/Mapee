namespace WorldEditor
{
    public class VersionedBlockStateReader : IObjectReader<ObjectReadParamter, IObject?>
    {
        public VersionList<IObjectReader<ObjectReadParamter, IObject?>> Versions { get; set; }

        public VersionedBlockStateReader()
        {
            Versions = new VersionList<IObjectReader<ObjectReadParamter, IObject?>>
            {
                { Version.Oldest, Version.Post_Beta_1_3, new AlphaBlockStateReader() },
                { Version.Post_1_1, Version.Snapshot_17w46a, new AnvilBlockStateChunkReader() },
                { Version.Snapshot_17w47a, Version.Newest, new BlockStateChunkReader() }
            };
        }

        public IObject? Read(ObjectReadParamter input)
        {
            if (!Versions.TryRetrieveValue(input.Version, out IObjectReader<ObjectReadParamter, IObject?>? reader)) return null;
            return reader.Read(input);
        }
    }
}
