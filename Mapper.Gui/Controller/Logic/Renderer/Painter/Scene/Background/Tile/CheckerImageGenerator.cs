using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using Mapper.Gui.Model;

namespace Mapper.Gui.Logic
{
    public class CheckerImageGenerator : ICheckerImageGenerator
    {
        private byte[]? _rawPixels;

        public ImageSource Generate(Size size, int checkerSize, ColorPair pair)
        {
            int length = (int)size.Width * (int)size.Height * 3;
            if (_rawPixels is null || _rawPixels.Length < length) 
            {
                _rawPixels = new byte[(int)size.Width * (int)size.Height * 3];
            }

            for (int y = 0; y < (int)size.Height; y++)
            {
                for (int x = 0; x < (int)size.Width; x++)
                {
                    bool isEven = x / checkerSize % 2 == 0 ^ y / checkerSize % 2 == 0;
                    Color color = isEven ? pair.Even : pair.Odd;

                    int offset = (y * (int)size.Width + x) * 3;
                    _rawPixels[offset] = color.R;
                    _rawPixels[offset + 1] = color.G;
                    _rawPixels[offset + 2] = color.B;
                }
            }

            WriteableBitmap bitmap = new((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Rgb24, null);
            bitmap.Lock();

            bitmap.WritePixels(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight), _rawPixels, bitmap.PixelWidth * bitmap.Format.BitsPerPixel / 8, 0);

            bitmap.Unlock();
            bitmap.Freeze();

            return bitmap;
        }
    }
}
