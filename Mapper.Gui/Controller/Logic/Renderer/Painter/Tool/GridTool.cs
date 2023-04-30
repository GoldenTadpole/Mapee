using Mapper.Gui.Model;
using System;
using System.Windows;
using System.Windows.Media;
using WorldEditor;

namespace Mapper.Gui.Logic
{
    public class GridTool : ToggleableTool, IPainter
    {
        public DrawingGroup? DrawingGroup
        {
            get => _drawingGroup;
            set
            {
                _drawingGroup = value;
                RenderOptions.SetEdgeMode(value, EdgeMode.Aliased);
            }
        }
        private DrawingGroup? _drawingGroup;

        public IScene Scene { get; }

        private Color _chunkLineColor;
        private readonly SolidColorBrush _chunkLinePenBrush;

        private readonly SolidColorBrush _regionReallyThinLinPenBrush;
        private Color _regionReallyThinLineColor;

        public Pen ChunkLinePen { get; set; }
        public Pen RegionDashedLinePen { get; set; }
        public Pen RegionDashedLinePenThin { get; set; }
        public Pen RegionLinePenThin { get; set; }
        public Pen RegionLinePenReallyThin { get; set; }

        public GridTool(IScene scene)
        {
            Scene = scene;

            byte r = 255, a = 192;

            _chunkLineColor = Color.FromArgb(a, r, r, r);
            _chunkLinePenBrush = new SolidColorBrush(_chunkLineColor);
            ChunkLinePen = new Pen(_chunkLinePenBrush, 1);

            RegionDashedLinePen = new Pen(new SolidColorBrush(Color.FromArgb((byte)(a * 0.66D), r, r, r)), 3)
            {
                DashStyle = new DashStyle(new double[] { 4, 3 }, 0)
            };
            RegionDashedLinePen.Freeze();

            RegionDashedLinePenThin = new Pen(new SolidColorBrush(Color.FromArgb((byte)(a * 0.66D), r, r, r)), 2)
            {
                DashStyle = new DashStyle(new double[] { 4, 3 }, 0)
            };
            RegionDashedLinePenThin.Freeze();

            RegionLinePenThin = new Pen(new SolidColorBrush(Color.FromArgb((byte)(a * 0.5D), r, r, r)), 2);
            RegionLinePenThin.Freeze();

            _regionReallyThinLineColor = Color.FromArgb((byte)(a * 0.66D), r, r, r);
            _regionReallyThinLinPenBrush = new SolidColorBrush(_regionReallyThinLineColor);
            RegionLinePenReallyThin = new Pen(_regionReallyThinLinPenBrush, 1);
        }

        public void Paint(DrawingContext drawingContext)
        {
            if (!Enabled || !IsTurnedOn) return;

            int zoomLevel = Scene.ZoomLevel;
            if (zoomLevel > -5)
            {
                double zoom = Scene.ZoomCoefficient * 0.33D;

                byte a;
                if (zoom > 1) a = _chunkLineColor.A;
                else a = (byte)(_chunkLineColor.A * zoom);

                if (a >= 80) a = 80;

                _chunkLinePenBrush.Color = Color.FromArgb(a, _chunkLineColor.R, _chunkLineColor.G, _chunkLineColor.B);
                RenderInterval(drawingContext, 16, (Pen)ChunkLinePen.GetAsFrozen(), 512);
            }

            Pen regionLinePen;
            if (zoomLevel < -8)
            {
                double zoom = Scene.ZoomCoefficient * 5;
                byte a = (byte)(_regionReallyThinLineColor.A * zoom);

                _regionReallyThinLinPenBrush.Color = Color.FromArgb(a, _regionReallyThinLineColor.R, _regionReallyThinLineColor.G, _regionReallyThinLineColor.B);

                regionLinePen = (Pen)RegionLinePenReallyThin.GetAsFrozen();
            }
            else if (zoomLevel < -4)
            {
                regionLinePen = RegionLinePenThin;
            }
            else if (zoomLevel < 0)
            {
                regionLinePen = RegionDashedLinePenThin;
            }
            else
            {
                regionLinePen = RegionDashedLinePen;
            }

            RenderInterval(drawingContext, 512, regionLinePen);
        }
        private void RenderInterval(DrawingContext drawingContext, int interval, Pen pen, int ignoreMod = 0)
        {
            Rect area = GetArea(interval);

            int xCount = IntervalMathUtilities.GetIntervalCount((int)area.X, (int)area.BottomRight.X, interval);
            int yCount = IntervalMathUtilities.GetIntervalCount((int)area.Y, (int)area.BottomRight.Y, interval);

            for (int x = 0; x < xCount; x++)
            {
                XzPoint point0 = new((int)area.X + x * interval, (int)area.Y);
                XzPoint point1 = new((int)area.X + x * interval, (int)area.BottomRight.Y + 1);

                if (ignoreMod != 0 && MathUtilities.NegMod((int)point0.X, ignoreMod) == 0) continue;

                drawingContext.DrawLine(pen, Scene.XzToPointOnScreen(point0), Scene.XzToPointOnScreen(point1));
            }

            for (int y = 0; y < yCount; y++)
            {
                XzPoint point0 = new((int)area.X, (int)(area.Y + y * interval));
                XzPoint point1 = new((int)area.BottomRight.X + 1, (int)(area.Y + y * interval));

                if (ignoreMod != 0 && MathUtilities.NegMod((int)point0.Z, ignoreMod) == 0) continue;

                drawingContext.DrawLine(pen, Scene.XzToPointOnScreen(point0), Scene.XzToPointOnScreen(point1));
            }
        }

        private Rect GetArea(int interval)
        {
            Point topLeft = GetLocation(Scene.XzToXy(Scene.TopLeft), interval);

            XzPoint xzPoint = Scene.BottomRight;
            Point bottomRight = Scene.XzToXy(new XzPoint(xzPoint.X + 1, xzPoint.Z + 1));

            return new Rect(topLeft, bottomRight);
        }
        private static Point GetLocation(Point scenePoint, int interval)
        {
            return new Point(MathUtilities.FindSectionY((int)scenePoint.X, interval) * interval,
                MathUtilities.FindSectionY((int)scenePoint.Y, interval) * interval);
        }
    }
}
