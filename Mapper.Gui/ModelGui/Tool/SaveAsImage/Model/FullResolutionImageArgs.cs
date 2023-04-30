using System.Windows.Media;

namespace Mapper.Gui.Model
{
    public readonly struct FullResolutionImageArgs
    {
        public bool ClipArea { get; init; }
        public bool CheckerPatternEnabled { get; init; }
        public Color BackgroundColor { get; init; }
    }
}
