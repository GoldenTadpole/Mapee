using Mapper.Gui.Model;
using System.Windows;
using System.Windows.Media;

namespace Mapper.Gui.Logic
{
    public class RegionPainter : IPainter<XzRange>
    {
        public DrawingGroup? DrawingGroup { get; set; }
        public IRegionScene RegionScene { get; set; }

        public RegionPainter(IRegionScene regionScene) 
        {
            RegionScene = regionScene;
        }

        public void Paint(DrawingContext drawingContext, XzRange visibleGrid)
        {
            if (DrawingGroup is null) return;

            for (int z = 0; z < visibleGrid.Size.Z; z++)
            {
                for (int x = 0; x < visibleGrid.Size.X; x++)
                {
                    XzPoint point = new(visibleGrid.TopLeftPoint.X + x, visibleGrid.TopLeftPoint.Z + z);
                    if (!RegionScene.TryGetRegion(point, out IRenderedRegion? region)) continue;
                    if (region is null || region.Image is null || region.AreaInRegion is null) continue;

                    Point xy = Map.TransformXzToXy(new XzPoint(region.Coords.X * 512 + region.AreaInRegion.Value.TopLeftPoint.X, region.Coords.Z * 512 + region.AreaInRegion.Value.TopLeftPoint.Z));
                    drawingContext.DrawImage(region.Image, new Rect(xy, new Size(region.Image.Width, region.Image.Height)));
                }
            }
        }
    }
}
