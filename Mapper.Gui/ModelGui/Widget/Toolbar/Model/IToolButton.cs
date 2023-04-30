using System.Windows.Controls;
using System.Windows.Media;

namespace Mapper.Gui.Model
{
    public interface IToolButton
    {
        IToggleableTool Tool { get; }
        ImageSource? Icon { get; }
        string? Name { get; }
        string? ToolTip { get; }
        Button? Button { get; }

        void SetButton(Button button);
    }
}
