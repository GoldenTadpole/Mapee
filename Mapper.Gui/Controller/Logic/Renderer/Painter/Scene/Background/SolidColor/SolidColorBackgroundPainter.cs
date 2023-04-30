using System.Windows.Media;

namespace Mapper.Gui.Logic
{
    public class SolidColorBackgroundPainter : IPainter<BackgroundPaintArgs>
    {
        public DrawingGroup? DrawingGroup { get; set; }

        public void Paint(DrawingContext drawingContext, BackgroundPaintArgs args)
        {
            if (DrawingGroup is null) return;
            drawingContext.DrawRectangle(new SolidColorBrush(args.Background.SolidColor), null, args.BackgroundArea);
        }
    }
}
