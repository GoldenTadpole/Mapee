using Mapper.Gui.Model;
using System.Windows.Media;

namespace Mapper.Gui.Logic
{
    public class ScenePainter : IScenePainter
    {
        public DrawingGroup? DrawingGroup 
        {
            get => ScalePainter.DrawingGroup;
            set 
            {
                ScalePainter.DrawingGroup = value;
                RegionPainter.DrawingGroup = value;
            }
        }
        public Renderer Renderer { get; }

        public IPainter<ScalePaintArgs> ScalePainter { get; set; }
        public IPainter<XzRange> RegionPainter { get; set; }

        public ScenePainter(Renderer renderer) 
        {
            Renderer = renderer;

            ScalePainter = new ScalePainter();
            RegionPainter = new RegionPainter(new RegionScene(Renderer.Scene.Domain));
        }

        public void Paint(DrawingContext drawingContext)
        {
            ScenePaintArgs args = new() 
            {
                Scale = new ScalePaintArgs() 
                {
                    Enabled = Renderer.Scene.Map.ScaleBehaviour.Enabled,
                    Offset = Renderer.Scene.Map.ScaleBehaviour.Offset,
                    ZoomLevel = Renderer.Scene.Map.ScaleBehaviour.CurrentZoomLevel,
                    ZoomCoefficient = Renderer.Scene.Map.ScaleBehaviour.CurrentZoomCoefficient,
                    LevelIncrement = Renderer.Scene.Map.ScaleBehaviour.LevelIncrement
                },
                VisibleArea = Renderer.Scene.Map.ProvideVisibleGrid()
            };

            Paint(drawingContext, args);
        }
        public void Paint(DrawingContext drawingContext, ScenePaintArgs args)
        {
            ScalePainter.Paint(drawingContext, args.Scale);
            RegionPainter.Paint(drawingContext, args.VisibleArea);
        }
    }
}
