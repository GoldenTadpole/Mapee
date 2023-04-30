using System.Windows;

namespace Mapper.Gui
{
    public readonly struct Dpi
    {
        public double DpiX { get; init; }
        public double DpiY { get; init; }

        public static double Default => 96;

        public Dpi(double dpiX, double dpiY) 
        {
            DpiX = dpiX;
            DpiY = dpiY;
        }
        public Dpi(DpiScale dpiScale)
        {
            DpiX = dpiScale.DpiScaleX * Default;
            DpiY = dpiScale.DpiScaleY * Default;
        }
    }
}
