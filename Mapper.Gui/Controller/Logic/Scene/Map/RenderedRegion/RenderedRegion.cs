using Mapper.Gui.Model;
using System.Windows.Media;

namespace Mapper.Gui.Logic
{
    public class RenderedRegion : IRenderedRegion
    {
        public XzPoint Coords { get; }
        public XzRange? AreaInRegion { get; }
        public ImageSource? Image { get; }

        public RenderedRegion(XzPoint coords, XzRange? areaInRegion, ImageSource? image)
        {
            Coords = coords;
            AreaInRegion = areaInRegion;
            Image = image;
        }
    }
}
