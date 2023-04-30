using Mapper.Gui.Model;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Mapper.Gui
{
    /// <summary>
    /// Interaction logic for RenderSettings.xaml
    /// </summary>
    public partial class RenderSettingsControl : Window
    {
        public RenderSettings RenderSettings { get; private set; }
        public bool DialogClosed { get; private set; } = true;

        public event EventHandler? RenderProfileUpdated;

        private string _dimensionName;

        public RenderSettingsControl(RenderSettings renderSettings, string dimensionName)
        {
            InitializeComponent();

            RenderSettings = renderSettings;

            SkyLightTextBox.Text = RenderSettings.SkyLightIntensity.ToString();
            AmbientLightTextBox.Text = RenderSettings.AmbientLightIntensity.ToString();
            AltitudeTextBox.Text = RenderSettings.AltitudeYOffset.ToString();
            StepIntensityTextBox.Text = RenderSettings.SemiTransparentStepIntensity.ToString();

            _dimensionName = dimensionName;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CustomFrameWindowInitializer frameInitializer = new(this, Template);
            frameInitializer.SetSecondaryTitle($"For: {_dimensionName}");

            CheckerBackgroundRadioBox.IsChecked = RenderSettings.Background.Type == BackgroundType.Checker;
            SolidBackgroundRadioBox.IsChecked = RenderSettings.Background.Type == BackgroundType.Solid;
        }

        private void Render_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;

            BackgroundType type = SolidBackgroundRadioBox.IsChecked is not null && SolidBackgroundRadioBox.IsChecked.Value ? BackgroundType.Solid : BackgroundType.Checker;
            RenderSettings = new()
            {
                SkyLightIntensity = ParseSingle(SkyLightTextBox.Text),
                AmbientLightIntensity = ParseSingle(AmbientLightTextBox.Text),
                AltitudeYOffset = ParseSingle(AltitudeTextBox.Text),
                SemiTransparentStepIntensity = ParseSingle(StepIntensityTextBox.Text),
                Background = new Background()
                {
                    Type = type,
                    CheckedColorPair = RenderSettings.Background.CheckedColorPair,
                    SolidColor = type == BackgroundType.Solid ? Colors.Black : RenderSettings.Background.SolidColor
                }
            };

            DialogClosed = false;
            RenderProfileUpdated?.Invoke(this, EventArgs.Empty);

            if (CloseWindowCheckBox.IsChecked ?? true) Close();
        }

        private bool ValidateInput()
        {
            if (!InputValidator.ValidateSingle(SkyLightTextBox.Text, 0, 1))
            {
                return InvalidInput.ShowMessage("Sky light must be a number between 0 and 1.");
            }
            if (!InputValidator.ValidateSingle(AmbientLightTextBox.Text, 0, 1))
            {
                return InvalidInput.ShowMessage("Ambient light must be a number between 0 and 1.");
            }
            if (!InputValidator.ValidateSingle(AltitudeTextBox.Text))
            {
                return InvalidInput.ShowMessage("Altitude must be a number.");
            }
            if (!InputValidator.ValidateSingle(StepIntensityTextBox.Text))
            {
                return InvalidInput.ShowMessage("Step intensity must be a number.");
            }

            return true;
        }

        private static float ParseSingle(string text) 
        {
            return float.Parse(text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture);
        }
    }
}
