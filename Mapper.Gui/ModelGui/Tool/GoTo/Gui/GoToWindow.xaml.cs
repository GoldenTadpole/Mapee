using Mapper.Gui.Model;
using System;
using System.Windows;

namespace Mapper.Gui
{
    /// <summary>
    /// Interaction logic for GoToWindow.xaml
    /// </summary>
    public partial class GoToWindow : Window
    {
        private XzPoint _playerPos, _playerSpawn, _worlSpawn;

        public XzPoint SelectedPoint { get; private set; }
        public bool DialogClosed { get; private set; } = true;

        private bool _closing = false;

        public GoToWindow(XzPoint selected, XzPoint playerPosition, XzPoint playerSpawn, XzPoint worldSpawn)
        {
            InitializeComponent();

            selected.X = Math.Ceiling(selected.X);
            selected.Z = Math.Ceiling(selected.Z);

            SelectedPoint = selected;
            _playerPos = playerPosition;
            _playerSpawn = playerSpawn;
            _worlSpawn = worldSpawn;

            CoordinateTextBox.Text = $"{selected}";
            PlayerPositionButton.Content = $"Player position ({playerPosition})";
            PlayerSpawnButton.Content = $"Player spawn ({playerSpawn})";
            WorldSpawnButton.Content = $"World spawn ({worldSpawn})";
        }

        private void GoToButton_Click(object sender, RoutedEventArgs e)
        {
            if (!TryParse(CoordinateTextBox.Text, out XzPoint point)) return;

            SelectedPoint = point;
            DialogClosed = false;

            _closing = true;
            Close();
        }

        private static bool TryParse(string input, out XzPoint point)
        {
            try
            {
                string[] split = input.Split(";");

                point = new XzPoint(int.Parse(split[0].Trim()), int.Parse(split[1].Trim()));
                return true;
            }
            catch
            {
                point = new XzPoint();
                return false;
            }
        }

        private void PlayerPositionButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedPoint = _playerPos;
            DialogClosed = false;

            _closing = true;
            Close();
        }
        private void PlayerSpawnButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedPoint = _playerSpawn;
            DialogClosed = false;

            _closing = true;
            Close();
        }
        private void WorldSpawnButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedPoint = _worlSpawn;
            DialogClosed = false;

            _closing = true;
            Close();
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            if (!_closing) Close();
        }
    }
}
