namespace WorldEditor
{
    public class ApiChunkLoadOptions
    {
        public ObjectLoadOptions BiomeOptions { get; set; }
        public ObjectLoadOptions BlockStateOptions { get; set; }
        public ObjectLoadOptions BlockLightOptions { get; set; }
        public ObjectLoadOptions SkyLightOptions { get; set; }
        public ObjectLoadOptions HeightmapOptions { get; set; }

        public bool KeepDataTag { get; set; }

        public ApiChunkLoadOptions()
        {
            BiomeOptions = new ObjectLoadOptions()
            {
                ObjectDeserializer = new VersionedBiomeReader()
            };
            BlockStateOptions = new ObjectLoadOptions()
            {
                ObjectDeserializer = new VersionedBlockStateReader()
            };
            BlockLightOptions = new ObjectLoadOptions()
            {
                ObjectDeserializer = new VersionedLightReader(LightType.BlockLight.ToString())
            };
            SkyLightOptions = new ObjectLoadOptions()
            {
                ObjectDeserializer = new VersionedLightReader(LightType.SkyLight.ToString())
            };
            HeightmapOptions = new ObjectLoadOptions()
            {
                ObjectDeserializer = new VersionedHeightmapReader()
            };

            KeepDataTag = true;
        }
    }
}
