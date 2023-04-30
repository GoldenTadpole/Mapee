namespace WorldEditor
{
    public class ObjectLoadOptions
    {
        public bool LoadObject { get; set; } = true;
        public bool KeepDataTag { get; set; } = true;
        public bool RemoveDataTag { get; set; } = false;

        public IObjectReader<ObjectReadParamter, IObject?>? ObjectDeserializer { get; set; }
    }
}
