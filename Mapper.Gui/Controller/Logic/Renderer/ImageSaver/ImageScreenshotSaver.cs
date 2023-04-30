using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

namespace Mapper.Gui.Logic
{
    public class ImageScreenshotSaver
    {
        public Control Control { get; }

        public ImageScreenshotSaver(Control control)
        {
            Control = control;
        }

        public Size GetSize() 
        {
            return new Size((int)Control.ActualWidth, (int)Control.ActualHeight);
        }
        public void SaveScreenshot(string path)
        {
            RenderTargetBitmap bitmap = new((int)Control.ActualWidth, (int)Control.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(Control);

            PngBitmapEncoder png = new();
            png.Frames.Add(BitmapFrame.Create(bitmap));

            using FileStream file = File.OpenWrite(path);
            png.Save(file);
        }
    }
}
