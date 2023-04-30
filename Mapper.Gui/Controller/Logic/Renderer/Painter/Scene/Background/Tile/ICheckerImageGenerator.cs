using Mapper.Gui.Model;
using System.Windows;
using System.Windows.Media;

namespace Mapper.Gui.Logic
{
    public interface ICheckerImageGenerator
    {
        ImageSource Generate(Size size, int checkerSize, ColorPair pair);
    }
}
