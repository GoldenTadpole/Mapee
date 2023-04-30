using Mapper.Gui.Model;

namespace Mapper.Gui.Logic
{
    public interface IRegionScene
    {
        bool TryGetRegion(XzPoint point, out IRenderedRegion? region);
    }
}
