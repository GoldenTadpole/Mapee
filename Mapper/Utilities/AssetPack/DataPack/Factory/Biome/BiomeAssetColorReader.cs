using System.Windows;
using System.Windows.Media;
using WorldEditor;

namespace Mapper
{
    public class BiomeAssetColorReader : IObjectReader<BiomeColorReadArgs, VecRgb>
    {
        public VecRgb Read(BiomeColorReadArgs input)
        {
            switch (input.BiomeColor.Type)
            {
                case BiomeColorType.HardCoded:
                    if (input.BiomeColor is not HardCodedBiomeColor hardCoded) return VecRgb.Empty;

                    Span<byte> bytes = stackalloc byte[sizeof(int)];
                    if (!BitConverter.TryWriteBytes(bytes, hardCoded.Color)) return VecRgb.Empty;

                    return Color.FromRgb(bytes[2], bytes[1], bytes[0]);
                case BiomeColorType.Colormapped:
                    if (input.BiomeColor is not ColormappedBiomeColor colormapped || input.Colormap is null) return VecRgb.Empty;
                    return GetColor(colormapped, input.Colormap);
                case BiomeColorType.Modified:
                default:
                    return VecRgb.Empty;
            }
        }

        private static VecRgb GetColor(ColormappedBiomeColor biome, IReadOnlyBitmap colormap)
        {
            Point pixel = GetPixel(biome, colormap);
            return colormap.GetPixel((int)pixel.X, (int)pixel.Y);
        }
        private static Point GetPixel(ColormappedBiomeColor biome, IReadOnlyBitmap colormap)
        {
            float ajdTemperature = float.Clamp(biome.Temperature, 0, 1);
            float adjDownfall = float.Clamp(biome.Downfall, 0, 1) * ajdTemperature;

            int x = (int)Math.Floor((1 - ajdTemperature) * colormap.Size.Width - 1);
            int y = (int)Math.Floor((1 - adjDownfall) * colormap.Size.Height - 1);

            return new Point(int.Clamp(x, 0, (int)colormap.Size.Width - 1), int.Clamp(y, 0, (int)colormap.Size.Height - 1));
        }
    }
}
