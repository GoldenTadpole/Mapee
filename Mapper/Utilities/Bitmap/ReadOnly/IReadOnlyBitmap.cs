using System.Windows;
using System.Windows.Media;

namespace Mapper
{
    public interface IReadOnlyBitmap
    {
        Size Size { get; }

        Color GetPixel(int x, int y);
        Color GetAverageColor();
        Color GetAverageColor(int x1, int y1, int width, int height);
    }
}
