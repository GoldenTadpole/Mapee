using Mapper.Gui.Model;
using System.Windows.Controls;
using System.Windows.Media;

namespace Mapper.Gui.Controller
{
    public class ToolButton : IToolButton
    {
        public IToggleableTool Tool { get; set; }
        public ImageSource? Icon { get; set; }
        public string? Name { get; set; }
        public string? ToolTip { get; set; }
        public Button? Button { get; set; }

        public ToolButton(IToggleableTool tool) 
        {
            Tool = tool;
        }

        public void SetButton(Button button)
        {
            Button = button;
        }
    }
}
