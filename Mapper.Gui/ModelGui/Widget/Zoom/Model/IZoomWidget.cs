using System;

namespace Mapper.Gui.Model
{
    public interface IZoomWidget
    {
        double ZoomPercentage { get; }

        event EventHandler? LevelChanged;

        void ZoomIn();
        void ZoomOut();
    }
}
