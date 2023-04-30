using System.Windows;

namespace Mapper.Gui.Model
{
    public interface IImageSaver
    {
        FullResolutionImageArgs DefaultFullResArgs { get; }

        Size GetScreenshotSize();
        Size GetFullResolutionSize(FullResolutionImageArgs args);

        void SaveAsScreenshot(string path);
        void SaveAsFullResolution(string path, FullResolutionImageArgs args);
    }
}
