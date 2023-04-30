using Mapper.Gui.Logic;
using Mapper.Gui.Model;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Mapper.Gui.Controller
{
    public class ScrollbarWidget : IScrollbarWidget
    {
        public Interval? LoadedArea 
        {
            get 
            {
                XzRange loaded = Scene.Map.RegionLoader.LoadedArea;
                if (Orientation == Orientation.Horizontal)
                {
                    return new Interval(loaded.TopLeftPoint.X, loaded.BottomRightPoint.X);
                }
                else
                {
                    return new Interval(loaded.TopLeftPoint.Z, loaded.BottomRightPoint.Z);
                }
            }
        }
        public Interval? VisibleArea 
        {
            get
            {
                if (Orientation == Orientation.Horizontal)
                {
                    return new Interval(Scene.Map.ScaleBehaviour.TopLeftPoint.X, Scene.Map.ScaleBehaviour.BottomRightPoint.X);
                }
                else
                {
                    return new Interval(Scene.Map.ScaleBehaviour.TopLeftPoint.Y, Scene.Map.ScaleBehaviour.BottomRightPoint.Y);
                }
            }
        }

        public event EventHandler? Update;

        public Scene Scene { get; }
        public Orientation Orientation { get; }

        public ScrollbarWidget(Scene scene, Renderer renderer, Orientation orientation) 
        {
            Scene = scene;
            Orientation = orientation;

            renderer.OnRenderUpdate += Renderer_RenderUpdate;
        }

        private void Renderer_RenderUpdate()
        {
            Update?.Invoke(this, EventArgs.Empty);
        }

        public void SetLeftMostVisiblePoint(double point)
        {
            Point pointInScene;
            if (Orientation == Orientation.Horizontal)
            {
                pointInScene = new Point(point, Scene.Map.ScaleBehaviour.TopLeftPoint.Y);
            }
            else 
            {
                pointInScene = new Point(Scene.Map.ScaleBehaviour.TopLeftPoint.X, point);
            }

            Scene.Map.ScaleBehaviour.SetTopLeftPoint(pointInScene);
        }
    }
}
