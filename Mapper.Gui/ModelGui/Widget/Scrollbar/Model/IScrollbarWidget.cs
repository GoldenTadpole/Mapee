using System;

namespace Mapper.Gui.Model
{
    public interface IScrollbarWidget
    {
        Interval? LoadedArea { get; }
        Interval? VisibleArea { get; }

        event EventHandler? Update;

        void SetLeftMostVisiblePoint(double point);
    }
}
