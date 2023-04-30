using System;
using System.Collections.Generic;
using WorldEditor;

namespace Mapper.Gui.Model
{
    public interface IDimensionWidget
    {
        IReadOnlyList<DimensionUI> Dimensions { get; }
        IReadOnlyList<Dimension> ExtraDimensions { get; }
        Dimension CurrentDimension { get; set; }

        event EventHandler? DimensionUpdate;
        event EventHandler? DimensionSelectionChanged;

        bool IsDimensionAllowed(Dimension dimension);
    }
}
