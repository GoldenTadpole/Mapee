using System;

namespace Mapper.Gui.Controller
{
    public readonly struct TextInfo
    {
        public IText Text { get; init; }
        public DateTime FirstAppeared { get; init; }

        public TextInfo(IText text, DateTime firstAppeared)
        {
            Text = text;
            FirstAppeared = firstAppeared;
        }
    }
}
