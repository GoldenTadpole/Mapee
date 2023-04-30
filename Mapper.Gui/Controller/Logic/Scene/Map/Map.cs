using Mapper.Gui.Model;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Mapper.Gui.Logic
{
    public class Map
    {
        public ScaleBehaviour ScaleBehaviour { get; }
        public IRegionLoader RegionLoader { get; }

        private static readonly int REGION_SIZE = 512;

        public Map(Control outputControl, IRegionLoader regionLoader)
        {
            ScaleBehaviour = new ScaleBehaviour(outputControl);
            RegionLoader = regionLoader;

            BackgroundWork.Run(TimeSpan.FromMilliseconds(100), LoadUnrenderedRegions);
        }

        private void LoadUnrenderedRegions()
        {
            RegionLoader.LoadArea(ProvideVisibleGrid());
        }
        public XzRange ProvideVisibleGrid()
        {
            XzPoint topLeft = TransformXyToXz(ScaleBehaviour.TopLeftPoint);
            XzPoint bottomRight = TransformXyToXz(ScaleBehaviour.BottomRightPoint);

            int xCount = IntervalMathUtilities.GetIntervalCount((int)topLeft.X, (int)bottomRight.X, REGION_SIZE);
            int zCount = IntervalMathUtilities.GetIntervalCount((int)topLeft.Z, (int)bottomRight.Z, REGION_SIZE);

            topLeft = new XzPoint(
                IntervalMathUtilities.FindIntervalPoint((int)topLeft.X, REGION_SIZE),
                IntervalMathUtilities.FindIntervalPoint((int)topLeft.Z, REGION_SIZE));

            return new XzRange()
            {
                TopLeftPoint = topLeft,
                BottomRightPoint = new XzPoint(topLeft.X + xCount - 1, topLeft.Z + zCount - 1),
            };
        }

        public void SetCenterPoint(XzPoint xz)
        {
            ScaleBehaviour.SetCenterPoint(TransformXzToXy(xz));
        }

        public static Point TransformXzToXy(XzPoint xz)
        {
            return new Point(xz.X, xz.Z);
        }
        public static XzPoint TransformXyToXz(Point point)
        {
            return new XzPoint(point.X, point.Y);
        }

        public Point TransformXzToPointOnScreen(XzPoint xz)
        {
            Point topLeft = ScaleBehaviour.TopLeftPoint;

            Point point = TransformXzToXy(xz);

            double x = point.X - topLeft.X;
            double y = point.Y - topLeft.Y;

            return new Point(x * ScaleBehaviour.CurrentZoomCoefficient, y * ScaleBehaviour.CurrentZoomCoefficient);
        }
        public XzPoint TransformPointOnScreenToXz(Point point)
        {
            Point topLeft = ScaleBehaviour.TopLeftPoint;

            double x = topLeft.X + point.X / ScaleBehaviour.CurrentZoomCoefficient;
            double y = topLeft.Y + point.Y / ScaleBehaviour.CurrentZoomCoefficient;

            if (x < 0) x = Math.Floor(x);
            if (y < 0) y = Math.Floor(y);

            return TransformXyToXz(new Point(x, y));
        }
    }
}
