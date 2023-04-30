using System.Windows.Media;

namespace Mapper.Gui.Model
{
    public struct Background
    {
        public BackgroundType Type { get; set; }
        public ColorPair CheckedColorPair { get; set; }
        public Color SolidColor { get; set; }

        public static Background Empty => new()
        {
            Type = BackgroundType.Checker,
            CheckedColorPair = ColorPair.Overworld
        };
    }
}
