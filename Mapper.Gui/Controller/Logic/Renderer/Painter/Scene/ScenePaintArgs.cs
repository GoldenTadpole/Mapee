using Mapper.Gui.Model;

namespace Mapper.Gui.Logic
{
    public readonly struct ScenePaintArgs
    {
        public ScalePaintArgs Scale { get; init; }
        public XzRange VisibleArea { get; init; }
    }
}
