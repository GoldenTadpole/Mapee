using System.Windows.Media;
using WorldEditor;

namespace Mapper.Gui
{
    public readonly struct WorldPanelEntry
    {
        public Level Level { get; }
        public ImageSource Icon { get; }

        public WorldPanelEntry(Level level, ImageSource icon)
        {
            Level = level;
            Icon = icon;
        }
    }
}
