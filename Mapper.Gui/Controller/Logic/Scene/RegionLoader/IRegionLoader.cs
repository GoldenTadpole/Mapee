using Mapper.Gui.Model;

namespace Mapper.Gui.Logic
{
    public interface IRegionLoader
    {
        XzRange LoadedArea { get; }
        void LoadArea(XzRange range);
    }
}
