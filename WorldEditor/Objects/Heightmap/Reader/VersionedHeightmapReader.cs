namespace WorldEditor
{
    public class VersionedHeightmapReader : IObjectReader<ObjectReadParamter, IObject?>
    {
        public VersionList<IObjectReader<ObjectReadParamter, IObject?>> Versions { get; set; }

        public VersionedHeightmapReader()
        {
            Versions = new VersionList<IObjectReader<ObjectReadParamter, IObject?>>
            {
                { Version.Oldest, Version.Snapshot_18w05a, new OldHeightmapReader() },
                { Version.Snapshot_18w06a, Version.Newest, new HeightmapReader() }
            };
        }

        public IObject? Read(ObjectReadParamter input)
        {
            if (!Versions.TryRetrieveValue(input.Version, out IObjectReader<ObjectReadParamter, IObject?>? reader)) return null;
            return reader.Read(input);
        }
    }
}
