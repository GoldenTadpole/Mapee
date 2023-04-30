using System;

namespace Mapper.Gui.Controller
{
    public interface IText
    {
        string DisplayedText { get; }
        ICooldown Cooldown => new StagedCooldown();

        event EventHandler? RemoveOrderInitiated;
    }
}
