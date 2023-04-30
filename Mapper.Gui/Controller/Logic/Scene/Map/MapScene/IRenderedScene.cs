using Mapper.Gui.Model;
using System;
using System.Collections.Generic;

namespace Mapper.Gui.Logic
{
    public interface IRenderedScene
    {
        event EventHandler Update;

        SceneInfo SceneParameter { get; }
        IDictionary<XzPoint, IRenderedRegion> RenderedRegions { get; }
    }
}
