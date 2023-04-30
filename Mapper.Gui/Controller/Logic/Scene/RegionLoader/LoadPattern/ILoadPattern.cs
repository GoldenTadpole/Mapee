using Mapper.Gui.Model;
using System.Collections.Generic;

namespace Mapper.Gui.Logic
{
    public interface ILoadPattern
    {
        IEnumerable<XzPoint> CreatePattern(XzRange area);
    }
}
