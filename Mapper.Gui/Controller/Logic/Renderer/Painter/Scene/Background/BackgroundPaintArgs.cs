using Mapper.Gui.Model;
using System.Windows;

namespace Mapper.Gui.Logic
{
    public readonly struct BackgroundPaintArgs
    {
        public Rect BackgroundArea { get; init; }
        public Background Background { get; init; }
    }
}
