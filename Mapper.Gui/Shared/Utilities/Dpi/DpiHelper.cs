using System.Windows;
using System.Windows.Media;

namespace Mapper.Gui
{
    public static class DpiHelper
    {
        public static Point CalibrateToDpiScale(this Point point) 
        {
            Size scaling = GetScaling();
            return new Point(point.X / scaling.Width, point.Y / scaling.Height);
        }
        public static Size GetScaling() 
        {
            Dpi dpi = GetDpi();
            return new Size(dpi.DpiX / Dpi.Default, dpi.DpiY / Dpi.Default);
        }
        public static Dpi GetDpi() 
        {
            if (Application.Current is null ||
                Application.Current.MainWindow is null ||
                !Application.Current.MainWindow.IsVisible) return new Dpi(Dpi.Default, Dpi.Default);

            return new Dpi(VisualTreeHelper.GetDpi(Application.Current.MainWindow));
        }
    }
}
