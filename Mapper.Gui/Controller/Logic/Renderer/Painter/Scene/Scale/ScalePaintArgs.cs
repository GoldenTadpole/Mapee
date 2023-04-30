using System.Windows;

namespace Mapper.Gui.Logic
{
    public readonly struct ScalePaintArgs
    {
        public bool Enabled { get; init; }
        public Point Offset { get; init; }
        public int ZoomLevel { get; init; }
        public double ZoomCoefficient { get; init; }
        public double LevelIncrement { get; init; }
    }
}
