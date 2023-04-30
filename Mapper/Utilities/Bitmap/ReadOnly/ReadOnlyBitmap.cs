using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Mapper
{
    public sealed class ReadOnlyBitmap : IReadOnlyBitmap
    {
        public BitmapSource Source { get; }
        public byte[] PixelData { get; }

        public Size Size => new(Source.PixelWidth, Source.PixelHeight);

        public ReadOnlyBitmap(Uri uri)
        {
            Source = new BitmapImage(uri);

            int stride = (int)Source.Width * 4;
            PixelData = new byte[stride * (int)Source.Height];

            FormatConvertedBitmap convertedBitmap = new FormatConvertedBitmap(Source, PixelFormats.Bgra32, null, 0);
            convertedBitmap.CopyPixels(PixelData, stride, 0);
        }
        public ReadOnlyBitmap(Stream stream)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = stream;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            Source = bitmapImage;
            FormatConvertedBitmap convertedBitmap = new FormatConvertedBitmap(bitmapImage, PixelFormats.Bgra32, null, 0);

            int stride = (convertedBitmap.PixelWidth * convertedBitmap.Format.BitsPerPixel + 7) / 8;
            PixelData = new byte[stride * bitmapImage.PixelHeight];
            convertedBitmap.CopyPixels(PixelData, stride, 0);
        }

        public Color GetPixel(int x, int y)
        {
            int offset = GetOffset(x, y);

            if (Source.Format == PixelFormats.Rgb24) 
            {
                return Color.FromRgb(PixelData[offset], PixelData[offset + 1], PixelData[offset + 2]);
            }

            return Color.FromArgb(PixelData[offset + 3], PixelData[offset + 2], PixelData[offset + 1], PixelData[offset]);
        }
        public Color GetAverageColor()
        {
            return GetAverageColor(0, 0, Source.PixelWidth, Source.PixelHeight);
        }
        public Color GetAverageColor(int x1, int y1, int width, int height)
        {
            int r = 0, g = 0, b = 0, a = 0;
            float opaqueCount = 0;

            for (int y = y1; y < y1 + height; y++)
            {
                for (int x = x1; x < x1 + width; x++)
                {
                    Color color = GetPixel(x, y);

                    if (color.A > 0)
                    {
                        r += color.R;
                        g += color.G;
                        b += color.B;
                        opaqueCount++;
                    }

                    a += color.A;
                }
            }

            float count = width * height;
            return Color.FromArgb((byte)(a / count), (byte)(r / opaqueCount), (byte)(g / opaqueCount), (byte)(b / opaqueCount));
        }

        private int GetOffset(int x, int y)
        {
            return (y * Source.PixelWidth + x) * 4;
        }
    }
}
