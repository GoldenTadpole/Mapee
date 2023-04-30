using Mapper.Gui.Model;
using System.Collections.Generic;

namespace Mapper.Gui.Logic
{
    public class MiddleRowLoadPattern : ILoadPattern
    {
        public IEnumerable<XzPoint> CreatePattern(XzRange area)
        {
            XzPoint size = area.Size;

            int lineZ = (int)size.Z / 2;
            int pow = 1;

            for (int i = 0; i < size.Z; i++)
            {
                int z = lineZ + i * pow;
                pow *= -1;
                lineZ = z;

                for (int x = 0; x < size.X; x++)
                {
                    yield return new XzPoint(area.TopLeftPoint.X + x, area.TopLeftPoint.Z + z);
                }
            }
        }
    }
}
