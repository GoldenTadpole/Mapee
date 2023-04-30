namespace WorldEditor
{
    public class VersionedLightReader : IObjectReader<ObjectReadParamter, IObject?>
    {
        public VersionList<IObjectReader<ObjectReadParamter, IObject?>> Versions { get; set; }

        public VersionedLightReader(string tagName)
        {
            Versions = new VersionList<IObjectReader<ObjectReadParamter, IObject?>>
            {
                { Version.Oldest, Version.Post_Beta_1_3, new OldLightReader(tagName) },
                { Version.Post_1_1, Version.Newest, new LightReader(tagName) }
            };
        }

        public IObject? Read(ObjectReadParamter input)
        {
            if (!Versions.TryRetrieveValue(input.Version, out IObjectReader<ObjectReadParamter, IObject?>? deserializer)) return null;

            return deserializer.Read(input);
        }
    }
}
