using System.Collections.Generic;

namespace Mapper.Gui.Model
{
    public interface IToolButtonSegment
    {
        public IList<IToolButton> Tools { get; }
        public double LeftGap { get; }
        public double RightGap { get; }
    }
}
