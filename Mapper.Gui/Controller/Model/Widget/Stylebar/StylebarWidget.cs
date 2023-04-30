using Mapper.Gui.Logic;
using Mapper.Gui.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mapper.Gui.Controller
{
    public class StylebarWidget : IStylebarWidget
    {
        public Scene Scene { get; }

        public IReadOnlyList<IStyle> Styles => _styles;
        private List<IStyle> _styles = new List<IStyle>();

        public string SelectedStyleId
        {
            get => Scene.Domain.CurrentStyle.Metadata.Id;
            set 
            {
                if(SelectedStyleId == value) return;

                foreach (Style style in Scene.Domain.Styles) 
                {
                    if (style.Metadata.Id != value) continue;

                    Scene.UpdateStyle(style);

                    foreach (IStyle baseStyle in _styles) 
                    {
                        if (baseStyle.Id != style.Metadata.Id) continue;

                        StyleCollectionChanged?.Invoke(this, baseStyle);
                        return;
                    }
                    return;
                }
            }
        }

        public event EventHandler<IStyle>? StyleCollectionChanged;

        public StylebarWidget(Scene scene) 
        {
            Scene = scene;
            SelectedStyleId = Scene.Domain.CurrentStyle.Metadata.Id;

            foreach (Style style in Scene.Domain.Styles) 
            {
                StyleAdapter adapter = new StyleAdapter(style);
                _styles.Add(adapter);
            }
        }
    }
}
