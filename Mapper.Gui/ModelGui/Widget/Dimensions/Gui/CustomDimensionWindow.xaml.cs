using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WorldEditor;

namespace Mapper.Gui
{
    /// <summary>
    /// Interaction logic for CustomDimensionWindow.xaml
    /// </summary>
    public partial class CustomDimensionWindow : Window
    {
        public Dimension SelectedDimension { get; private set; }
        public bool DialogClosed { get; private set; } = true;

        private bool _closing = false;

        public CustomDimensionWindow(IReadOnlyList<Dimension> dimensions, Dimension selectedDimension)
        {
            InitializeComponent();

            SelectedDimension = selectedDimension;
            AddButtons(dimensions);
        }

        private void AddButtons(IReadOnlyList<Dimension> dimensions) 
        {
            double width = 150;

            for (int i = 0; i < dimensions.Count; i++) 
            {
                Dimension dimension = dimensions[i];

                Button button = new()
                {
                    Content = dimension.Namespace,
                    FontSize = 14,
                    ToolTip = $"Go to {dimension.Name}",
                    Height = double.NaN,
                    Cursor = Cursors.Hand
                };
                if (dimension == SelectedDimension) button.FontWeight = FontWeights.UltraBold;

                button.Click += (sender, e) =>
                {
                    SelectedDimension = dimension;
                    DialogClosed = false;

                    _closing = true;
                    Close();
                };

                ButtonContainer.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(25, GridUnitType.Pixel)
                });

                Grid.SetRow(button, ButtonContainer.RowDefinitions.Count - 1);
                ButtonContainer.Children.Add(button);

                button.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                Height += ButtonContainer.RowDefinitions[^1].Height.Value;
                if (button.DesiredSize.Width + 50 > width) width = button.DesiredSize.Width + 50;
            }

            Height += 3;
            Width = width;
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            if (!_closing) Close();
        }
    }
}
