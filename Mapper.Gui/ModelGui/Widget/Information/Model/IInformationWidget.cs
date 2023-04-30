using System;
using System.Windows.Controls;

namespace Mapper.Gui.Model
{
    public interface IInformationWidget
    {
        Control MainControl { get; }

        event EventHandler<SceneInformation>? InformationUpdate;
        event EventHandler<XzPoint>? CursorUpdate;
    }
}
