namespace Mapper
{
    public readonly struct Light
    {
        public byte SkyLight { get; init; }
        public byte BlockLight { get; init; }

        public Light(byte skyLight, byte blockLight)
        {
            SkyLight = skyLight;
            BlockLight = blockLight;
        }
    }
}
