using Mapper.Gui.Logic;
using Mapper.Gui.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WorldEditor;

namespace Mapper.Gui.Logic
{
    public class HighlightChunkTool : ToggleableTool, IPainter
    {
        public DrawingGroup? DrawingGroup { get; set; }

        public IScene Scene { get; set; }
        public IRenderInvoker RenderInvoker { get; set; }
        public Control OutputControl { get; set; }

        public Brush ChunkBrush { get; set; }

        private XzPoint _prevChunk = new(int.MinValue, int.MinValue);
        private bool _isTurnedDown = false;

        public HighlightChunkTool(IScene scene, IRenderInvoker renderInvoker, Control outputControl)
        {
            Scene = scene;
            RenderInvoker = renderInvoker;
            OutputControl = outputControl;

            ChunkBrush = new SolidColorBrush(Color.FromArgb(192, 255, 221, 0));
            ChunkBrush.Freeze();

            Scene.ZoomChanged += Scene_ZoomChanged;
            OutputControl.MouseDown += Control_MouseDown;
            OutputControl.MouseUp += Control_MouseUp;
            OutputControl.MouseMove += Control_MouseMove;
            OutputControl.MouseLeave += Control_MouseLeave;
        }

        public void Paint(DrawingContext drawingContext)
        {
            if (!Enabled || !IsTurnedOn || _prevChunk.X == int.MinValue) return;
            if (!OutputControl.IsVisible) return;

            XzPoint chunk = FindChunk(Mouse.GetPosition(OutputControl));

            XzPoint topLeft = new(chunk.X * 16, chunk.Z * 16);
            XzPoint bottomRight = new(topLeft.X + 16, topLeft.Z + 16);

            Rect rectangle = new(Scene.XzToPointOnScreen(topLeft), Scene.XzToPointOnScreen(bottomRight));
            drawingContext.DrawRectangle(ChunkBrush, null, rectangle);
        }

        private void Scene_ZoomChanged(object? sender, EventArgs e) 
        {
            Enabled = Scene.ZoomLevel > -5;
        }
        private void Control_MouseDown(object sender, EventArgs e) 
        {
            _isTurnedDown = true;
        }
        private void Control_MouseUp(object sender, EventArgs e)
        {
            _isTurnedDown = false;
        }
        private void Control_MouseMove(object sender, EventArgs e)
        {
            if (!Enabled || !IsTurnedOn || _isTurnedDown) return;

            XzPoint chunk = FindChunk(Mouse.GetPosition(OutputControl));
            if (chunk.Equals(_prevChunk)) return;

            _prevChunk = chunk;
            RenderInvoker.Render();
        }
        private void Control_MouseLeave(object sender, EventArgs e)
        {
            if (!Enabled || !IsTurnedOn) return;

            _prevChunk = new XzPoint(int.MinValue, int.MinValue);
            _isTurnedDown = false;

            RenderInvoker.Render();
        }

        private XzPoint FindChunk(Point point)
        {
            XzPoint pointInScene = Scene.PointOnScreenToXz(point);
            return new XzPoint(MathUtilities.FindSectionY((int)pointInScene.X, 16), MathUtilities.FindSectionY((int)pointInScene.Z, 16));
        }
    }
}
