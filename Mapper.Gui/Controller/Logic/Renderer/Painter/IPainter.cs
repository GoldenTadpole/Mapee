using System.Windows.Media;

namespace Mapper.Gui.Logic
{
    public interface IPainter
    {
        DrawingGroup? DrawingGroup { get; set; }
        void Paint(DrawingContext drawingContext);
    }
    public interface IPainter<TArgs>
    {
        DrawingGroup? DrawingGroup { get; set; }
        void Paint(DrawingContext drawingContext, TArgs args);
    }
}
