using System.Collections.Generic;
using System.Windows.Controls;

namespace Mapper.Gui
{
    public class MapViewerArgs
    {
        public Control Canvas { get; set; }
        public Control Footer { get; set; }
        public Control HorizontalScrollbar { get; set; }
        public Control VerticalScrollbar { get; set; }

        public IList<Control> Widgets { get; set; }

        public MapViewerArgs(Control canvas, Control footer, Control horizontalScrollbar, Control verticalScrollbar) 
        {
            Canvas = canvas;
            Footer = footer;
            HorizontalScrollbar = horizontalScrollbar;
            VerticalScrollbar = verticalScrollbar;

            Widgets = new List<Control>();
        }
    }
}
