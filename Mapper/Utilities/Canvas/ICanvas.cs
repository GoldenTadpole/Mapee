using System.Windows.Media;
using WorldEditor;

namespace Mapper
{
    public interface ICanvas : IDisposable
    {
        Coords TopLeftPoint { get; }

        void SetPixel(int xInWorld, int zInWorld, VecRgb color);
        ImageSource? GetBitmap();
    }
}
