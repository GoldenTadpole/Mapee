using System;
using System.Collections.Generic;

namespace Mapper.Gui.Model
{
    public interface IStylebarWidget
    {
        IReadOnlyList<IStyle> Styles { get; }
        string SelectedStyleId { get; set; }

        event EventHandler<IStyle>? StyleCollectionChanged;
    }
}
