using System;
using System.Windows;

namespace Mapper.Gui
{
    /// <summary>
    /// Interaction logic for AddBlock.xaml
    /// </summary>
    public partial class AddBlock : Window
    {
        public string NameResult { get; private set; }
        public bool DialogClosed { get; private set; } = true;

        private bool _closing = false;

        public AddBlock(string name)
        {
            InitializeComponent();

            NameResult = name;
            NameTextBox.Text = name;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NameTextBox.Focus();
            NameTextBox.CaretIndex = NameTextBox.Text.Length;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NameResult = NameTextBox.Text;

            DialogClosed = false;
            _closing = true;
            Close();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
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
