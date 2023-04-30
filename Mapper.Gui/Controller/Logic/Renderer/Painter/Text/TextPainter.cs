using Mapper.Gui.Logic;
using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Mapper.Gui.Controller
{
    public class TextPainter : IPainter
    {
        public DrawingGroup? DrawingGroup { get; set; }
        public IRenderInvoker RenderInvoker { get; set; }
        public Control Control { get; set; }

        public TextInfo? CurrentText { get; private set; }

        private FormattedText? _currentFormattedText = null;
        private bool _updatePending = false;

        private const byte _textBrighness = 192;

        public TextPainter(IRenderInvoker renderInvoker, Control control) 
        {
            RenderInvoker = renderInvoker;
            Control = control;

            InitializeLoop();
        }

        private void InitializeLoop()
        {
            new Thread(() =>
            {
                double lastOpacity = double.MinValue;

                while (true)
                {
                    if (CurrentText is not null && _updatePending)
                    {
                        _updatePending = false;
                        RenderInvoker?.Render();
                    }
                    else if (CurrentText is not null)
                    {
                        double opacity = CurrentText.Value.Text.Cooldown.GetOpacity(CurrentText.Value.FirstAppeared);
                        if (lastOpacity != opacity)
                        {
                            lastOpacity = opacity;
                            RenderInvoker?.Render();
                        }
                    }

                    Thread.Sleep(50);
                }
            })
            {
                IsBackground = true
            }.Start();
        }

        public void Paint(DrawingContext drawingContext)
        {
            if (DrawingGroup is null || CurrentText is null || _currentFormattedText is null) return;

            Point point = new Point(
                (Control.ActualWidth - _currentFormattedText.Width) / 2,
                (Control.ActualHeight - _currentFormattedText.Height) / 2);

            double opacity = CurrentText.Value.Text.Cooldown.GetOpacity(CurrentText.Value.FirstAppeared);

            Brush textBrush = new SolidColorBrush(Color.FromArgb((byte)(255 * opacity), _textBrighness, _textBrighness, _textBrighness));

            _currentFormattedText.SetForegroundBrush(textBrush);

            DrawTextBackground(drawingContext, point, opacity);
            drawingContext.DrawText(_currentFormattedText, point);
        }

        private void DrawTextBackground(DrawingContext drawingContext, Point point, double opacity)
        {
            if (_currentFormattedText is null) return;

            Brush textBackgroundBrush = new SolidColorBrush(Color.FromArgb((byte)(192 * opacity), 24, 24, 24));
            textBackgroundBrush.Freeze();

            drawingContext.DrawRectangle(textBackgroundBrush, null, new Rect(point.X - 5, point.Y - 1, _currentFormattedText.Width + 10, _currentFormattedText.Height + 1));
        }

        public void SetText(IText? text) 
        {
            if (text is null) 
            {
                CurrentText = null;
                _currentFormattedText = null;    

                return;
            }

            TextInfo info = new TextInfo(text, DateTime.Now);
            CurrentText = info;
            _currentFormattedText = CreateText(text.DisplayedText, new SolidColorBrush(Color.FromRgb(_textBrighness, _textBrighness, _textBrighness)));

            text.RemoveOrderInitiated += Text_RemoveOrderInitiated;

            _updatePending = true;
        }
        private static FormattedText CreateText(string value, Brush brush)
        {
            return new FormattedText(
                value,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(new FontFamily("Consolas, Segoe UI"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal),
                20,
                brush,
                96);
        }

        private void Text_RemoveOrderInitiated(object? sender, EventArgs e) 
        {
            if (CurrentText is not null) 
            {
                CurrentText.Value.Text.RemoveOrderInitiated -= Text_RemoveOrderInitiated;
                RenderInvoker?.Render();
            }
        }
    }
}
