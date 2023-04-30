using Mapper.Gui.Logic;
using Mapper.Gui.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Mapper.Gui.Controller
{
    public class InformationWidget : IInformationWidget
    {
        public Control MainControl => Renderer.GraphicsCanvas;

        public event EventHandler<SceneInformation>? InformationUpdate;
        public event EventHandler<XzPoint>? CursorUpdate;

        private readonly SceneInformation _information = new();

        public Scene Scene { get; }
        public Renderer Renderer { get; }

        public InformationWidget(Scene scene, Renderer renderer) 
        {
            Scene = scene;
            Renderer = renderer;
            Renderer.OnRenderUpdate += Renderer_Update;
            Renderer.GraphicsCanvas.MouseMove += GraphicsCanvas_MouseMove;
            Renderer.GraphicsCanvas.MouseLeave += GraphicsCanvas_MouseLeave;
        }

        private void Renderer_Update() 
        {
            InformationUpdate?.Invoke(this, GetInformation());
        }
        private void GraphicsCanvas_MouseMove(object sender, MouseEventArgs e) 
        {
            CursorUpdate?.Invoke(this, Scene.Map.TransformPointOnScreenToXz(Mouse.GetPosition(Renderer.GraphicsCanvas)));
        }
        private void GraphicsCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            CursorUpdate?.Invoke(this, Scene.Map.TransformPointOnScreenToXz(Mouse.GetPosition(Renderer.GraphicsCanvas)));
        }

        private SceneInformation GetInformation()
        {
            _information.LoadedArea = Scene.Domain.CurrentWorld is not null ? Scene.RegionLoader.LoadedArea : null;
            _information.CursorOnBlock = Scene.Map.TransformPointOnScreenToXz(Mouse.GetPosition(Renderer.GraphicsCanvas));
            _information.VisibleArea = new XzRange()
            {
                TopLeftPoint = Map.TransformXyToXz(Scene.Map.ScaleBehaviour.TopLeftPoint),
                BottomRightPoint = Map.TransformXyToXz(Scene.Map.ScaleBehaviour.BottomRightPoint)
            };
            _information.LoadedRegions = Scene.Domain.CurrentWorld?.CurrentDimension.Scene.RenderedRegions.Count ?? 0;
            _information.WorldInformation = Scene.Domain.CurrentWorld?.Level;
            _information.CurrentDimension = Scene.Domain.CurrentWorld?.CurrentDimension.Dimension;

            return _information;
        }
    }
}
