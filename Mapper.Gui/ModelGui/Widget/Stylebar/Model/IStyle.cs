using System.Windows.Media;

namespace Mapper.Gui.Model
{
    public interface IStyle
    {
        ImageSource Icon { get; }
        string Name { get; }
        string Id { get; }
    }
}
