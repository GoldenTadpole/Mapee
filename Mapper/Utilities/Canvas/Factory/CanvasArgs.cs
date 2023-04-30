using System.Windows;
using WorldEditor;

namespace Mapper
{
    public readonly struct CanvasArgs
    {
        public Coords TopLeft { get; init; }
        public Size Size { get; init; }
        public Direction Direction { get; init; }

        public CanvasArgs(Coords topleft, Size size, Direction direction = Direction.North)
        {
            TopLeft = topleft;
            Size = size;
            Direction = direction;
        }
    }
}
