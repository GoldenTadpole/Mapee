using Mapper.Gui.Model;
using System;
using System.Windows;
using WorldEditor;

namespace Mapper.Gui.Logic
{
    public interface IScene
    {
        XzPoint TopLeft { get; }
        XzPoint BottomRight { get; }

        int ZoomLevel { get; }
        double ZoomCoefficient { get; }
        bool OffsetEnabled { get; set; }
        
        event EventHandler? ZoomChanged;

        bool IsSceneEmpty { get; }
        Dimension Dimension { get; }

        event EventHandler? DimensionChanged;

        Point XzToPointOnScreen(XzPoint xz);
        XzPoint PointOnScreenToXz(Point point);

        Point XzToXy(XzPoint xz);
        XzPoint XyToXz(Point point);
    }
}
