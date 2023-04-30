using Mapper.Gui.Model;
using System.Collections.Generic;

namespace Mapper.Gui.Logic
{
    public class SpiralLoadPattern : ILoadPattern
    {
        private static readonly XzPoint[] DIRECTION_VECTORS = new[] { new XzPoint(-1, 0), new XzPoint(0, -1), new XzPoint(1, 0), new XzPoint(0, 1) };
        private static readonly int DIRECTION_RIGHT = 3, DIRECTION_DOWN = 2;

        public IEnumerable<XzPoint> CreatePattern(XzRange area)
        {
            XzPoint size = area.Size;

            int x = (int)size.X / 2, z = (int)size.Z / 2;
            int loopRadius = 1;

            int count = 0;
            while (count < (int)size.X * (int)size.Z)
            {
                for (int direction = 0; direction < DIRECTION_VECTORS.Length; direction++)
                {
                    for (int i = 0; i < loopRadius; i++)
                    {
                        MoveInDirection(direction, ref x, ref z);
                        if (!area.IsWithinBoundsIgnoreCorner(x, z)) continue;

                        count++;
                        yield return new XzPoint(x + area.TopLeftPoint.X, z + area.TopLeftPoint.Z);
                    }
                }

                MoveDiagonal(ref x, ref z);
                loopRadius += 2;
            }
        }

        private static void MoveDiagonal(ref int x, ref int z)
        {
            MoveInDirection(DIRECTION_RIGHT, ref x, ref z);
            MoveInDirection(DIRECTION_DOWN, ref x, ref z);
        }
        private static void MoveInDirection(int direction, ref int x, ref int z)
        {
            x += (int)DIRECTION_VECTORS[direction].X;
            z += (int)DIRECTION_VECTORS[direction].Z;
        }
    }
}
