using System.Collections.Generic;

namespace Mapper.Gui.Model
{
    public interface IToolbarWidget
    {
        public IList<IToolButtonSegment> ToolButtonSegments { get; }
    }
}
