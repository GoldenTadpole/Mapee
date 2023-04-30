namespace WorldEditor
{
    public class VersionedBiomeReader : IObjectReader<ObjectReadParamter, IObject?>
    {
        public VersionList<IObjectReader<ObjectReadParamter, IObject?>> Versions { get; set; }

        public VersionedBiomeReader()
        {
            Versions = new VersionList<IObjectReader<ObjectReadParamter, IObject?>>
            {
                { Version.Oldest, Version.experimentalSnapshot_1_18_experimentalSnapshot_7, new OldBiomeReader() },
                { Version.Snapshot_21w37a, Version.Newest, new BiomeChunkReader() }
            };
        }

        public IObject? Read(ObjectReadParamter input)
        {
            if (!Versions.TryRetrieveValue(input.Version, out IObjectReader<ObjectReadParamter, IObject?>? deserializer)) return null;
            return deserializer.Read(input);
        }
    }
}
