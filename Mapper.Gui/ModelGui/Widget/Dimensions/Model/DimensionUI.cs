using System.Windows.Media;
using WorldEditor;

namespace Mapper.Gui.Model
{
    public class DimensionUI
    {
        public Dimension Dimension { get; set; }
        public ImageSource Icon { get; set; }

        public DimensionUI(Dimension dimension, ImageSource icon) 
        {
            Dimension = dimension;
            Icon = icon;
        }
    }
}
