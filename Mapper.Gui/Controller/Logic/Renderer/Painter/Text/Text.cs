using System;

namespace Mapper.Gui.Controller
{
    public class Text : IText
    {
        public string DisplayedText { get; set; }
        public ICooldown Cooldown { get; set; } = new StagedCooldown();

        public event EventHandler? RemoveOrderInitiated;

        public Text(string displayedText)
        {
            DisplayedText = displayedText;
        }
    }
}
