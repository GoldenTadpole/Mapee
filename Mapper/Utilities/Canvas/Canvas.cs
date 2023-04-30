using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WorldEditor;

namespace Mapper
{
    public class Canvas : ICanvas
    {
        public Coords TopLeftPoint { get; private set; }
        public Size Size { get; private set; }
        public Direction Direction { get; private set; }

        private readonly PixelTransformation _pixelTransformation;
        private readonly byte[] _pixelData;
        private bool _isEmpty = true;

        public Canvas(Coords topleft, Size size, Direction direction = Direction.North)
        {
            TopLeftPoint = topleft;
            Size = size;
            Direction = direction;

            _pixelTransformation = new PixelTransformation(Direction, TopLeftPoint, Size);
            _pixelData = new byte[(int)size.Width * (int)size.Height * 4];
        }

        public void SetPixel(int xInWorld, int zInWorld, VecRgb color)
        {
            _pixelTransformation.TransformPixelCoords(xInWorld, zInWorld, out int xP, out int yP);

            int offset = (yP * (int)Size.Height + xP) * 4;
            Rgb rgb = color.ToByteRgb();

            _pixelData[offset] = rgb.B;
            _pixelData[offset + 1] = rgb.G;
            _pixelData[offset + 2] = rgb.R;
            _pixelData[offset + 3] = byte.MaxValue;
            _isEmpty = false;
        }

        public ImageSource? GetBitmap()
        {
            if (_isEmpty) return null;

            WriteableBitmap output = new((int)Size.Height, (int)Size.Width, 96, 96, PixelFormats.Bgra32, null);
            output.Lock();

            int stride = (int)output.Width * (output.Format.BitsPerPixel / 8);
            output.WritePixels(new Int32Rect(0, 0, (int)output.Width, (int)output.Height), _pixelData, stride, 0);
            
            output.Unlock();
            output.Freeze();

            return output;
        }

        public void Dispose() { }
    }
}
