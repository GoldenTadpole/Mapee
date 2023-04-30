using Mapper.Gui.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Mapper.Gui
{
    /// <summary>
    /// Interaction logic for DimensionButtonPanel.xaml
    /// </summary>
    public partial class DimensionButtonPanel : UserControl
    {
        public DimensionUI Dimension { get; }

        public DimensionButtonPanel(DimensionUI dimension)
        {
            InitializeComponent();

            Dimension = dimension;
            DimensionIcon.Source = Dimension.Icon;
            ToolTip = $"Go to {Dimension.Dimension.Name}";
        }

        private void DimensionIcon_MouseMove(object sender, MouseEventArgs e)
        {
            ImageContainer.Background = Brushes.White;
        }
        private void DimensionIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageContainer.Background = Brushes.White;
        }
        private void DimensionIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageContainer.Background = Brushes.Transparent;
        }

        public void Select()
        {
            DimensionBorder.BorderThickness = new Thickness(2);
        }
        public void Deselect()
        {
            DimensionBorder.BorderThickness = new Thickness();
        }
    }
}
