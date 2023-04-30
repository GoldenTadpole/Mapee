using Mapper.Gui.Model;
using System.Windows;
using WorldEditor;

namespace Mapper.Gui.Logic
{
    public class DimensionDomain
    {
        public Dimension Dimension { get; set; }
        public IRenderedScene Scene { get; set; }
        public Scaling Scaling { get; set; }

        public HeightmapSettings HeightmapSettings { get; set; }
        public RenderSettings RenderSettings { get; set; }

        public DimensionDomain(IRenderedScene renderedScene)
        {
            Scene = renderedScene;
            Scaling = new Scaling(0, new Point(0, 0));
        }
    }
}
