using System;

namespace Mapper.Gui.Controller
{
    public class StagedCooldown : ICooldown
    {
        public TimeSpan Full { get; set; } = TimeSpan.FromSeconds(1.5D);
        public TimeSpan Partial { get; set; } = TimeSpan.FromSeconds(1.5D);

        public double GetOpacity(DateTime initialized)
        {
            TimeSpan passed = DateTime.Now - initialized;

            if (passed <= Full) return 1;
            if ((passed - Full) > Partial) return 0;
            return 1 - (passed - Full).Ticks / (double)Partial.Ticks;
        }
    }
}
