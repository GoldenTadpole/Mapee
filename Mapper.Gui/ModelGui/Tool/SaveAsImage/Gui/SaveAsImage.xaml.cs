using Mapper.Gui.Model;
using System;
using System.Windows;
using System.Windows.Media;

namespace Mapper.Gui
{
    /// <summary>
    /// Interaction logic for SaveAsImage.xaml
    /// </summary>
    public partial class SaveAsImageControl : Window
    {

        private readonly IImageSaver _imageSaver;

        public SaveAsImageControl(IImageSaver imageSaver)
        {
            InitializeComponent();

            _imageSaver = imageSaver;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _ = new CustomFrameWindowInitializer(this, Template);

            if (ScreenshotRadioButton.IsChecked is not null && ScreenshotRadioButton.IsChecked.Value)
            {
                SetScreenshotDescritpion();
                SetFullResolutionPanelEnabled(false);
            }
            else
            {
                SetFullScreenResolutionDescritpion();
                SetFullResolutionPanelEnabled(false);
            }

            SetScreenshotLabel();
            SetFullResolutionLabel();
        }

        private void SetScreenshotLabel() 
        {
            Size size = _imageSaver.GetScreenshotSize();
            ScreenshotRadioButton.Content = $"Screenshot  ({size.Width}x{size.Height})";
        }
        private void SetFullResolutionLabel()
        {
            Size size = _imageSaver.GetFullResolutionSize(CreateFullResolutionImageArgs());
            FullResolutionButton.Content = $"Full resolution  ({size.Width}x{size.Height})";
        }

        private void ScreenshotRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            SetScreenshotDescritpion();
            SetFullResolutionPanelEnabled(false);
        }
        private void FullResolutionButton_Checked(object sender, RoutedEventArgs e)
        {
            SetFullScreenResolutionDescritpion();
            SetFullResolutionPanelEnabled(true);
        }

        private void SetFullResolutionPanelEnabled(bool enabled)
        {
            if (FullResolutionPanel is null) return;
            FullResolutionPanel.Visibility = enabled ? Visibility.Visible : Visibility.Hidden;
        }
        private void ClipAreaCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (CheckerPatternRadioBox is not null && BlackColorRadioBox is not null) 
            {
                SetFullResolutionLabel();
            }
        }
        private void ClipAreaCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (CheckerPatternRadioBox is not null && BlackColorRadioBox is not null)
            {
                SetFullResolutionLabel();
            }
        }

        private void SetScreenshotDescritpion()
        {
            string description = "Captures the screen. Some image scaling artifacts may be present.";

            if (DescriptionLabel is null) return;
            DescriptionLabel.Text = $"Description: {description}";
        }
        private void SetFullScreenResolutionDescritpion()
        {
            string description = "Saves the image as one pixel per block. Gridlines will note be visible. Image size can be rather large.";

            if (DescriptionLabel is null) return;
            DescriptionLabel.Text = $"Description: {description}";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveDialog = new()
            {
                FileName = "RenderedMap",
                DefaultExt = ".png",
                Filter = "Image files (.png)|*.png"
            };

            bool? result = saveDialog.ShowDialog();
            if (result == null || !result.Value) return;

            string path = saveDialog.FileName;

            if (ScreenshotRadioButton.IsChecked is not null && ScreenshotRadioButton.IsChecked.Value)
            {
                SaveScreenshot(path);
            }
            else
            {
                SaveFullResolution(path);
            }

            Close();
        }

        private void SaveScreenshot(string path)
        {
            _imageSaver.SaveAsScreenshot(path);
        }
        private void SaveFullResolution(string path)
        {
            _imageSaver.SaveAsFullResolution(path, CreateFullResolutionImageArgs());
        }

        private FullResolutionImageArgs CreateFullResolutionImageArgs() 
        {
            return new FullResolutionImageArgs()
            {
                ClipArea = ClipAreaCheckBox.IsChecked is not null && ClipAreaCheckBox.IsChecked.Value,
                CheckerPatternEnabled = CheckerPatternRadioBox.IsChecked is not null && CheckerPatternRadioBox.IsChecked.Value,
                BackgroundColor = BlackColorRadioBox.IsChecked is not null && BlackColorRadioBox.IsChecked.Value ? Colors.Black : Color.FromArgb(0, 0, 0, 0)
            };
        }
    }
}
