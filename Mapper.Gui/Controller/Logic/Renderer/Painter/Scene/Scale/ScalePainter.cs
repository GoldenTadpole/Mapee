using System;
using System.Windows.Media;

namespace Mapper.Gui.Logic
{
    public class ScalePainter : IPainter<ScalePaintArgs>
    {
        public DrawingGroup? DrawingGroup { get; set; }

        private readonly TranslateTransform _translateTransform = new();
        private readonly ScaleTransform _scaleTransform = new();

        public void Paint(DrawingContext drawingContext, ScalePaintArgs args)
        {
            if (!args.Enabled || DrawingGroup is null) return;

            _translateTransform.X = args.Offset.X;
            _translateTransform.Y = args.Offset.Y;

            _scaleTransform.ScaleX = args.ZoomCoefficient;
            _scaleTransform.ScaleY = args.ZoomCoefficient;

            drawingContext.PushTransform(_translateTransform);
            drawingContext.PushTransform(_scaleTransform);

            RenderOptions.SetBitmapScalingMode(DrawingGroup, CalibrateScaling(args.ZoomLevel, args.LevelIncrement));
            RenderOptions.SetEdgeMode(DrawingGroup, EdgeMode.Aliased);
        }
        private static BitmapScalingMode CalibrateScaling(int level, double levelIncrement)
        {
            BitmapScalingMode mode;

            int center = -(int)Math.Ceiling(Math.Log(DpiHelper.GetScaling().Width, levelIncrement));

            if (level >= center) mode = BitmapScalingMode.NearestNeighbor;
            else if (level >= center - 5) mode = BitmapScalingMode.LowQuality;
            else mode = BitmapScalingMode.Fant;

            return mode;
        }
    }
}
