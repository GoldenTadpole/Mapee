using Mapper.Gui.Model;
using System;

namespace Mapper.Gui.Logic
{
    public class RegionScene : IRegionScene
    {
        public ProgramDomain Domain { get; }

        public RegionScene(ProgramDomain domain) 
        {
            Domain = domain;
        }

        public bool TryGetRegion(XzPoint point, out IRenderedRegion? region)
        {
            if (Domain.CurrentWorld is null) 
            {
                region = null;
                return false;
            }

            return Domain.CurrentWorld.CurrentDimension.Scene.RenderedRegions.TryGetValue(point, out region);
        }
    }
}
