using Mapper.Gui.Model;
using System.Windows.Media;

namespace Mapper.Gui.Logic
{
    public interface IRenderedRegion
    {
        XzPoint Coords { get; }
        XzRange? AreaInRegion { get; }
        ImageSource? Image { get; }
    }
}
