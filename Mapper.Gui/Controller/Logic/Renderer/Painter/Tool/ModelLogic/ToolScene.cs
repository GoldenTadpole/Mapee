using Mapper.Gui.Model;
using System;
using System.Windows;
using WorldEditor;

namespace Mapper.Gui.Logic
{
    public class ToolScene : IScene
    {
        public XzPoint TopLeft => XyToXz(Scene.Map.ScaleBehaviour.TopLeftPoint);
        public XzPoint BottomRight => XyToXz(Scene.Map.ScaleBehaviour.BottomRightPoint);

        public int ZoomLevel => Scene.Map.ScaleBehaviour.CurrentZoomLevel;
        public double ZoomCoefficient => Scene.Map.ScaleBehaviour.CurrentZoomCoefficient;
        public bool OffsetEnabled
        {
            get => Scene.Map.ScaleBehaviour.OffsetEnabled;
            set => Scene.Map.ScaleBehaviour.OffsetEnabled = value;
        }
        
        public event EventHandler? ZoomChanged 
        {
            add 
            {
                Scene.Map.ScaleBehaviour.ZoomChanged += value;
            }
            remove 
            {
                Scene.Map.ScaleBehaviour.ZoomChanged -= value;
            }
        }

        public bool IsSceneEmpty => Scene.Domain.CurrentWorld is null;
        public Dimension Dimension => Scene.Domain.CurrentWorld is null ? default : Scene.Domain.CurrentWorld.CurrentDimension.Dimension;

        public event EventHandler? DimensionChanged;

        public Scene Scene { get; }

        public ToolScene(Scene scene) 
        {
            Scene = scene;
            Scene.WorldChanged += Scene_WorldChanged;
            Scene.DimensionChanged += Scene_DimensionChanged;
        }

        private void Scene_WorldChanged(WorldDomain? old, WorldDomain current)
        {
            DimensionChanged?.Invoke(this, EventArgs.Empty);
        }
        private void Scene_DimensionChanged(DimensionDomain old, DimensionDomain current) 
        {
            DimensionChanged?.Invoke(this, EventArgs.Empty);
        }

        public XzPoint PointOnScreenToXz(Point point) => Scene.Map.TransformPointOnScreenToXz(point);
        public Point XzToPointOnScreen(XzPoint xz) => Scene.Map.TransformXzToPointOnScreen(xz);

        public XzPoint XyToXz(Point point) => Map.TransformXyToXz(point);
        public Point XzToXy(XzPoint xz) => Map.TransformXzToXy(xz);
    }
}
