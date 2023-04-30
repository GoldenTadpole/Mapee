using Mapper.Gui.Model;
using System.Collections.Generic;

namespace Mapper.Gui.Logic
{
    public class TopLeftLoadPattern : ILoadPattern
    {
        public IEnumerable<XzPoint> CreatePattern(XzRange area)
        {
            for (int z = (int)area.TopLeftPoint.Z; z <= area.BottomRightPoint.Z; z++)
            {
                for (int x = (int)area.TopLeftPoint.X; x <= area.BottomRightPoint.X; x++)
                {
                    yield return new XzPoint(x, z);
                }
            }
        }
    }
}
