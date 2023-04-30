using System.Windows;

namespace Mapper.Gui.Logic
{
    public class Scaling
    {
        public double ZoomLevel { get; set; }
        public Point CenterPoint { get; set; }

        public Scaling(double zoomLevel, Point centerPoint)
        {
            ZoomLevel = zoomLevel;
            CenterPoint = centerPoint;
        }
    }
}
