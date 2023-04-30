using Mapper.Gui.Model;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WorldEditor;

namespace Mapper.Gui
{
    /// <summary>
    /// Interaction logic for DimensionControl.xaml
    /// </summary>
    public partial class DimensionControl : UserControl
    {
        public IDimensionWidget Dimensions { get; }

        private List<DimensionButtonPanel> _dimensions = new List<DimensionButtonPanel>();
        private DimensionButtonPanel? _extraDimensionButton = null;

        public DimensionControl(IDimensionWidget dimensions)
        {
            InitializeComponent();

            Dimensions = dimensions;
            Dimensions.DimensionUpdate += Dimension_DimensionUpdate;
            Dimensions.DimensionSelectionChanged += Dimension_DimensionSelectionChanged;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetDimensions();
        }

        private void SetDimensions()
        {
            DimensionContainer.ColumnDefinitions.Clear();
            DimensionContainer.Children.Clear();
            _dimensions.Clear();

            foreach (DimensionUI dimension in Dimensions.Dimensions)
            {
                DimensionButtonPanel dimensionButton = CreateDimensionControl(dimension);

                dimensionButton.MouseDown += Dimension_MouseDown;
                if (dimension.Dimension == Dimensions.CurrentDimension)
                {
                    dimensionButton.Select();
                }
            }

            if (Dimensions.ExtraDimensions.Count < 1) return;

            _extraDimensionButton = CreateDimensionControl(new DimensionUI(new Dimension("", "custom dimensions"), new BitmapImage(new Uri("/Resources/Image/Dimension/ExtraDimensions_32px.png", UriKind.Relative))));
            if (IsCustomDimensionSelected()) _extraDimensionButton.Select();

            _extraDimensionButton.MouseDown += ExtraDimension_MouseDown;
        }
        private bool IsCustomDimensionSelected() 
        {
            foreach (Dimension dimension in Dimensions.ExtraDimensions) 
            {
                if(dimension == Dimensions.CurrentDimension) return true;
            }

            return false;
        }

        private DimensionButtonPanel CreateDimensionControl(DimensionUI dimension)
        {
            DimensionButtonPanel button = new DimensionButtonPanel(dimension)
            {
                Margin = new Thickness(4, 0, 0, 0),
                IsEnabled = Dimensions.IsDimensionAllowed(dimension.Dimension)
            };

            DimensionContainer.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(0, GridUnitType.Auto)
            });

            Grid.SetColumn(button, DimensionContainer.ColumnDefinitions.Count - 1);
            DimensionContainer.Children.Add(button);
            _dimensions.Add(button);

            return button;
        }

        private void Dimension_MouseDown(object? sender, EventArgs e) 
        {
            if (sender is null || sender is not DimensionButtonPanel button) return;
            Dimensions.CurrentDimension = button.Dimension.Dimension;
        }
        private void ExtraDimension_MouseDown(object? sender, EventArgs e)
        {
            CustomDimensionWindow window = new CustomDimensionWindow(Dimensions.ExtraDimensions, Dimensions.CurrentDimension);

            Point startupLocation = PointToScreen(new(0, 0)).CalibrateToDpiScale();
            startupLocation.Y += ActualHeight + 5;
            startupLocation.X += ActualWidth - window.Width;

            window.Top = startupLocation.Y;
            window.Left = startupLocation.X;

            window.Show();
            window.Closing += (s, ee) =>
            {
                if (window.DialogClosed) return;
                Dimensions.CurrentDimension = window.SelectedDimension;
            };
        }

        private void Dimension_DimensionUpdate(object? sender, EventArgs e) 
        {
            SetDimensions();
        }
        private void Dimension_DimensionSelectionChanged(object? sender, EventArgs e) 
        {
            foreach (DimensionButtonPanel button in _dimensions) 
            {
                if (button.Dimension.Dimension == Dimensions.CurrentDimension)
                {
                    button.Select();
                }
                else 
                {
                    button.Deselect();
                }
            }

            if (IsCustomDimensionSelected()) _extraDimensionButton?.Select();
        }
    }
}
