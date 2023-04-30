using System.Windows.Controls.Primitives;
using System.Windows;
using Mapper.Gui.Model;

namespace Mapper.Gui
{
    public static class ScrollbarUtilities
    {
        public static void AdjustScrollbar(ScrollBar scrollbarControl, IScrollbarWidget scrollbar, double trackLength) 
        {
            if (scrollbar.LoadedArea is null || scrollbar.VisibleArea is null ||
                scrollbarControl.Track is null || scrollbar.LoadedArea.Value.IsEmpty())
            {
                scrollbarControl.Visibility = Visibility.Hidden;
                return;
            }

            scrollbarControl.Minimum = scrollbar.LoadedArea.Value.Point1;
            scrollbarControl.Maximum = scrollbar.LoadedArea.Value.Point2 - scrollbar.VisibleArea.Value.Size + 1;

            double portSize = FindViewportSize(scrollbar.VisibleArea.Value.Size / scrollbar.LoadedArea.Value.Size * trackLength, trackLength, scrollbarControl.Maximum, scrollbarControl.Minimum);
            if (scrollbar.LoadedArea.Value.Size > 0 && !double.IsNaN(portSize) && portSize > 0)
            {
                scrollbarControl.ViewportSize = portSize;
                scrollbarControl.Value = scrollbar.VisibleArea.Value.Point1;
                scrollbarControl.Visibility = Visibility.Visible;
            }
            else
            {
                scrollbarControl.Visibility = Visibility.Hidden;
            }

            static double FindViewportSize(double thumbSize, double trackLength, double max, double min)
            {
                return (-thumbSize * max + thumbSize * min) / (thumbSize - trackLength);
            }
        }
    }
}
