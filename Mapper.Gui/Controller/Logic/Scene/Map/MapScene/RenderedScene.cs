using CommonUtilities.Collections.Observable;
using CommonUtilities.Collections.Synchronized;
using Mapper.Gui.Model;
using System;
using System.Collections.Generic;

namespace Mapper.Gui.Logic
{
    public class RenderedScene : IRenderedScene
    {
        public SceneInfo SceneParameter { get; }
        public IDictionary<XzPoint, IRenderedRegion> RenderedRegions { get; }

        public event EventHandler? Update;

        public RenderedScene(SceneInfo mapParameter)
        {
            SceneParameter = mapParameter;
            ObservableDictionary<XzPoint, IRenderedRegion> renderedRegions = new(new SynchronizedDictionary<XzPoint, IRenderedRegion>());

            RenderedRegions = renderedRegions;
            renderedRegions.CollectionChanged += OnRenderedRegionsCollectionChanged;
        }

        private void OnRenderedRegionsCollectionChanged(object? sender, EventArgs e)
        {
            Update?.Invoke(this, EventArgs.Empty);
        }
    }
}
