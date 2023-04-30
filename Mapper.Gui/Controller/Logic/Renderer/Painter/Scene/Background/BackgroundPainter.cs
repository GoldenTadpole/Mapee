using Mapper.Gui.Model;
using System.Windows;
using System.Windows.Media;

namespace Mapper.Gui.Logic
{
    public class BackgroundPainter : IBackgroundPainter
    {
        public DrawingGroup? DrawingGroup
        {
            get => TilePainter.DrawingGroup;
            set
            {
                TilePainter.DrawingGroup = value;
                SolidColorPainter.DrawingGroup = value;
            }
        }

        public IPainter<BackgroundPaintArgs> TilePainter { get; set; }
        public IPainter<BackgroundPaintArgs> SolidColorPainter { get; set; }
        public Renderer Renderer { get; set; }

        public BackgroundPainter(IPainter<BackgroundPaintArgs> tilePainter, IPainter<BackgroundPaintArgs> solidColorPainter, Renderer renderer)
        {
            TilePainter = tilePainter;
            SolidColorPainter = solidColorPainter;
            Renderer = renderer;
        }

        public void Paint(DrawingContext drawingContext)
        {
            BackgroundPaintArgs args = new()
            {
                BackgroundArea = new Rect(0, 0, Renderer.GraphicsCanvas.ActualWidth, Renderer.GraphicsCanvas.ActualHeight),
                Background = Renderer.Scene.Domain.CurrentWorld?.CurrentDimension.RenderSettings.Background ?? Background.Empty

            };

            Paint(drawingContext, args);
        }
        public void Paint(DrawingContext drawingContext, BackgroundPaintArgs args)
        {
            if (args.Background.Type == Model.BackgroundType.Checker) TilePainter.Paint(drawingContext, args);
            else SolidColorPainter.Paint(drawingContext, args);
        }
    }
}
