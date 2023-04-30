namespace WorldEditor
{
    public class ApiChunkReader : IObjectReader<ChunkParamater, ApiChunk?>
    {
        public virtual ApiChunkLoadOptions LoadOptions { get; set; }
        public virtual IChunkVersionFinder VersionFinder { get; set; }

        protected readonly struct ReturnedObject
        {
            public IObject? Object { get; }
            public bool CancelChunk { get; }

            public ReturnedObject(IObject? obj, bool cancelChunk)
            {
                Object = obj;
                CancelChunk = cancelChunk;
            }
        }

        public ApiChunkReader()
        {
            LoadOptions = new ApiChunkLoadOptions();
            VersionFinder = new ChunkVersionFinder();
        }

        public virtual ApiChunk? Read(ChunkParamater input)
        {
            Version version = VersionFinder.FindVersion(input);

            ApiChunk output = new()
            {
                Version = version,
                Level = LoadOptions.KeepDataTag ? input.Level : null,
            };

            ChunkUtilities.ReadCoordinates(input.Level, output.Version, output);

            ReturnedObject blockState = ReadObject(LoadOptions.BlockStateOptions, version, input);
            if (blockState.CancelChunk) return null;
            else output.BlockState = blockState.Object;

            ReturnedObject biome = ReadObject(LoadOptions.BiomeOptions, version, input);
            if (biome.CancelChunk) return null;
            else output.Biome = biome.Object;

            ReturnedObject blockLight = ReadObject(LoadOptions.BlockLightOptions, version, input);
            if (blockLight.CancelChunk) return null;
            else output.BlockLight = blockLight.Object;

            ReturnedObject skyLight = ReadObject(LoadOptions.SkyLightOptions, version, input);
            if (skyLight.CancelChunk) return null;
            else output.SkyLight = skyLight.Object;

            ReturnedObject heightmap = ReadObject(LoadOptions.HeightmapOptions, version, input);
            if (heightmap.CancelChunk) return null;
            else output.Heightmap = heightmap.Object;

            return output;
        }
        protected virtual ReturnedObject ReadObject(ObjectLoadOptions options, Version version, ChunkParamater input)
        {
            if (options.LoadObject && options.ObjectDeserializer is not null)
            {
                ObjectReadParamter parameter = new(input.Level, version, options.KeepDataTag);
                IObject? obj = options.ObjectDeserializer.Read(parameter);
                if (obj is null) return new ReturnedObject(null, false);

                return new ReturnedObject(obj, parameter.CancelChunk);
            }

            return new ReturnedObject(null, false);
        }
    }
}
