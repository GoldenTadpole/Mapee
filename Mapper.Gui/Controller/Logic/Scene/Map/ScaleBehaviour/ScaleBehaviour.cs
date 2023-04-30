using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Mapper.Gui.Logic
{
    public class ScaleBehaviour
    {
        public bool Enabled { get; set; } = true;
        public bool OffsetEnabled { get; set; } = true;
        public bool ZoomEnabled { get; set; } = true;

        public event EventHandler? OffsetChanged;
        public event EventHandler? ZoomChanged;

        public Point Offset
        {
            get => _offset;
            set
            {
                if (_offset == value) return;
                _offset = value;

                OffsetChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        private Point _offset = new(0, 0);

        private bool _mouseDown = false;
        private Point _prevMousePoint = new();
        private Point _prevOffset = new();

        public int MaxZoomLevels { get; set; } = 20;
        public int MinZoomLevels { get; set; } = -20;

        public double LevelIncrement { get; set; } = 1 / (1 + 18D / -120);
        public double CurrentZoomCoefficient { get; private set; } = 1;
        public int CurrentZoomLevel
        {
            get => _currentLevel;
            set
            {
                if (_currentLevel == value) return;
                _currentLevel = value;

                ZoomChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        private int _currentLevel = 0;

        public Point TopLeftPoint => new(-_offset.X / CurrentZoomCoefficient, -_offset.Y / CurrentZoomCoefficient);
        public Point BottomRightPoint
        {
            get
            {
                Point topLeft = TopLeftPoint;
                return new Point(topLeft.X + (_outputControl.ActualWidth / CurrentZoomCoefficient - 1), topLeft.Y + _outputControl.ActualHeight / CurrentZoomCoefficient - 1);
            }
        }

        public DrawingGroup? DrawingGroup { get; set; }
        private readonly Control _outputControl;

        public ScaleBehaviour(Control outputControl)
        {
            _outputControl = outputControl;

            MouseHook.MouseDown += MouseDown;
            MouseHook.MouseMove += MouseMove;
            MouseHook.MouseUp += MouseUp;

            _outputControl.MouseWheel += OnCanvasMouseWheel;
            _outputControl.SizeChanged += OnCanvasSizeChanged;
            _outputControl.KeyDown += Window_OnKeyDown;

            _outputControl.Loaded += (sender, e) =>
            {
                Window.GetWindow(_outputControl).KeyDown += Window_OnKeyDown;
            };
        }

        private void MouseDown(object? sender, Point point)
        {
            if (!OffsetEnabled || !_outputControl.IsMouseOver) return;

            _mouseDown = true;
            _prevMousePoint = point;
            _prevOffset = Offset;
        }
        private void MouseMove(object? sender, Point point)
        {
            if (!OffsetEnabled) return;
            if (!_mouseDown) return;

            Point moved = new(point.X - _prevMousePoint.X, point.Y - _prevMousePoint.Y);

            Offset = new(_prevOffset.X + moved.X, _prevOffset.Y + moved.Y);
        }
        private void MouseUp(object? sender, Point point)
        {
            _mouseDown = false;
        }

        private void OnCanvasMouseWheel(object? sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0) ZoomIn(GetZoomCenterPoint());
            else ZoomOut(GetZoomCenterPoint());
        }

        private void ZoomCanvas(Point point, double toCoefficient)
        {
            Point offset = ZoomOffset(_offset, point, 1 / CurrentZoomCoefficient);
            offset = ZoomOffset(offset, point, toCoefficient);

            Offset = new(offset.X, offset.Y);
        }
        private static Point ZoomOffset(Point currentOffset, Point point, double zoomFactor)
        {
            double offsetX = currentOffset.X, offsetY = currentOffset.Y;

            offsetX *= zoomFactor;
            offsetY *= zoomFactor;

            offsetX += (point.X * (1 / zoomFactor) - point.X) * zoomFactor;
            offsetY += (point.Y * (1 / zoomFactor) - point.Y) * zoomFactor;

            return new(offsetX, offsetY);
        }

        public void ZoomIn(Point point, int levels = 1)
        {
            if (!ZoomEnabled || CurrentZoomLevel >= MaxZoomLevels || levels == 0) return;

            double zoomFactor = LevelIncrement;
            zoomFactor = Math.Pow(zoomFactor, levels);

            ZoomCanvas(point, CurrentZoomCoefficient * zoomFactor);
            CurrentZoomCoefficient *= zoomFactor;

            CurrentZoomLevel += levels;
        }
        public void ZoomOut(Point point, int levels = 1)
        {
            if (!ZoomEnabled || CurrentZoomLevel <= MinZoomLevels || levels == 0) return;

            double zoomFactor = 1 / LevelIncrement;
            zoomFactor = Math.Pow(zoomFactor, levels);

            ZoomCanvas(point, CurrentZoomCoefficient * zoomFactor);
            CurrentZoomCoefficient *= zoomFactor;

            CurrentZoomLevel -= levels;
        }

        private void OnCanvasSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Point current = new(_outputControl.ActualWidth / 2, _outputControl.ActualHeight / 2);
            Point prev = new(e.PreviousSize.Width / 2, e.PreviousSize.Height / 2);
            Point move = new(current.X - prev.X, current.Y - prev.Y);

            Offset = new(_offset.X + move.X, _offset.Y + move.Y);
        }

        private static readonly Key[] _directionKeys = new Key[] { Key.Up, Key.Right, Key.Down, Key.Left };
        private static readonly int[,] _directionVectors = new int[,] { { 0, 1 }, { -1, 0 }, { 0, -1 }, { 1, 0 } };
        private static readonly int _moveBy = 16;

        private void Window_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.R))
            {
                ResetZoom();
            }

            for (int i = 0; i < _directionKeys.Length; i++)
            {
                if (Keyboard.IsKeyDown(_directionKeys[i]))
                {
                    Offset = new(_offset.X + _moveBy * _directionVectors[i, 0], _offset.Y + _moveBy * _directionVectors[i, 1]);
                }
            }
        }

        public void ResetZoom()
        {
            Point point = new(_outputControl.ActualWidth / 2, _outputControl.ActualHeight / 2);

            double zoomFactor = 1 / CurrentZoomCoefficient;
            CurrentZoomCoefficient = 1;
            CurrentZoomLevel = 0;
            ZoomCanvas(point, zoomFactor);
        }
        public void SetTopLeftPoint(Point point)
        {
            Offset = new(-point.X * CurrentZoomCoefficient, -point.Y * CurrentZoomCoefficient);
        }
        public void SetCenterPoint(Point point)
        {
            point.X *= -CurrentZoomCoefficient;
            point.Y *= -CurrentZoomCoefficient;

            point.X += _outputControl.ActualWidth / 2;
            point.Y += _outputControl.ActualHeight / 2;

            Offset = point;
        }

        public void SetScaling(Scaling scaling)
        {
            Point center = new(_outputControl.ActualWidth / 2, _outputControl.ActualHeight / 2);

            ResetZoom();
            if (scaling.ZoomLevel > 0)
            {
                ZoomOut(center, -Math.Abs((int)scaling.ZoomLevel));
            }
            else
            {
                ZoomIn(center, -Math.Abs((int)scaling.ZoomLevel));
            }

            SetCenterPoint(scaling.CenterPoint);
        }
        public void SaveScaling(Scaling output)
        {
            output.ZoomLevel = CurrentZoomLevel;
            output.CenterPoint = (BottomRightPoint - TopLeftPoint) / 2 + TopLeftPoint;
        }

        protected virtual Point GetZoomCenterPoint()
        {
            return Mouse.GetPosition(_outputControl);
        }
    }
}
