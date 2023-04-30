using System.Windows;

namespace Mapper.Gui
{
    public static class InvalidInput
    {
        public static bool ShowMessage(string message)
        {
            MessageBox.Show(message, "Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            return false;
        }
    }
}
