using Mapper.Gui.Model;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Mapper.Gui
{
    /// <summary>
    /// Interaction logic for ZoomControl.xaml
    /// </summary>
    public partial class ZoomControl : UserControl
    {
        public IZoomWidget Zoom { get; }

        public ZoomControl(IZoomWidget zoom)
        {
            InitializeComponent();

            Zoom = zoom;
            Zoom.LevelChanged += Zoom_LevelChanged;

            SetLabel(Zoom.ZoomPercentage);
        }

        private void Zoom_LevelChanged(object? sender, EventArgs e) 
        {
            SetLabel(Zoom.ZoomPercentage);
        }

        private void DecreaseZoomButton_Click(object sender, RoutedEventArgs e)
        {
            Zoom.ZoomOut();
        }
        private void IncreaseZoomButton_Click(object sender, RoutedEventArgs e)
        {
            Zoom.ZoomIn();
        }

        private void SetLabel(double percentage) 
        {
            percentage = Math.Round(percentage, 4);

            if (percentage > 1)
            {
                ZoomPercentageLabel.Content = $"{percentage.ToString("N1").Replace(",", ".")}x";
            }
            else
            {
                ZoomPercentageLabel.Content = $"{(int)(percentage * 100)}%";
            }
        }
    }
}
