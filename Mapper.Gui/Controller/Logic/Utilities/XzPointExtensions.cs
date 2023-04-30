using Mapper.Gui.Model;
using WorldEditor;

namespace Mapper.Gui.Logic
{
    public static class XzPointExtensions
    {
        public static Coords ToCoords(this XzPoint point)
        {
            return new Coords((int)point.X, (int)point.Z);
        }
    }
}
