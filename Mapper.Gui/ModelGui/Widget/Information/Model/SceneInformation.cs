using WorldEditor;

namespace Mapper.Gui.Model
{
    public class SceneInformation
    {
        public Dimension? CurrentDimension { get; set; }

        public XzRange? LoadedArea { get; set; }
        public XzRange VisibleArea { get; set; }
        public XzPoint CursorOnBlock { get; set; }

        public int LoadedRegions { get; set; }

        public Level? WorldInformation { get; set; }
    }
}
