using System.Windows;
using WorldEditor;

namespace Mapper
{
    public class PixelTransformation
    {
        public Direction Direction { get; set; }
        public Coords TopLeft { get; set; }
        public Size Size { get; set; }

        public PixelTransformation(Direction direction, Coords topLeft, Size size)
        {
            Direction = direction;
            TopLeft = topLeft;
            Size = size;
        }

        public void TransformPixelCoords(int xInWorld, int zInWorld, out int x, out int y)
        {
            switch (Direction)
            {
                case Direction.North:
                    x = xInWorld - (int)(TopLeft.X - Size.Height + 1);
                    y = zInWorld - TopLeft.Z;
                    break;
                case Direction.East:
                    x = zInWorld - TopLeft.Z;
                    y = TopLeft.X - xInWorld;
                    break;
                case Direction.South:
                    x = TopLeft.X - xInWorld;
                    y = (int)(TopLeft.Z + Size.Width) - zInWorld - 1;
                    break;
                default:
                    x = (int)(TopLeft.Z + Size.Width) - zInWorld - 1;
                    y = xInWorld - (int)(TopLeft.X - Size.Height + 1);
                    break;
            }
        }
    }
}
