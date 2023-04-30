using Mapper.Gui.Model;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Mapper.Gui
{
    /// <summary>
    /// Interaction logic for HorizontalScrollbarControl.xaml
    /// </summary>
    public partial class HorizontalScrollbarControl : UserControl
    {
        public IScrollbarWidget Scrollbar { get; }

        private bool _preventScrollBarUpdate = false;

        public HorizontalScrollbarControl(IScrollbarWidget scrollbar)
        {
            InitializeComponent();

            Scrollbar = scrollbar;
            Scrollbar.Update += Scrollbar_Update;
        }

        private void DockPanel_Loaded(object sender, RoutedEventArgs e)
        {
            SetScrollbar();
        }
        private void ScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_preventScrollBarUpdate) return;

            _preventScrollBarUpdate = true;
            Scrollbar.SetLeftMostVisiblePoint(e.NewValue);
            _preventScrollBarUpdate = false;
        }

        private void Scrollbar_Update(object? sender, EventArgs e)
        {
            _preventScrollBarUpdate = true;
            SetScrollbar();
            _preventScrollBarUpdate = false;
        }
        private void SetScrollbar()
        {
            ScrollbarUtilities.AdjustScrollbar(ScrollbarControl, Scrollbar, ScrollbarControl.Track.ActualWidth);
        }
    }
}
