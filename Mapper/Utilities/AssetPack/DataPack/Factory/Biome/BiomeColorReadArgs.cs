using WorldEditor;

namespace Mapper
{
    public readonly struct BiomeColorReadArgs
    {
        public IBiomeColor BiomeColor { get; init; }
        public IReadOnlyBitmap? Colormap { get; init; }

        public BiomeColorReadArgs(IBiomeColor biomeColor, IReadOnlyBitmap? colormap)
        {
            BiomeColor = biomeColor;
            Colormap = colormap;
        }
    }
}
