using System;

namespace Mapper.Gui
{
    public class TimedUpdater
    {
        public TimeSpan Interval { get; set; } = TimeSpan.FromMilliseconds(100);
        public bool PreviousSkipped { get; private set; }

        private DateTime _prev = DateTime.Now;

        public void Update(Action action) 
        {
            if ((DateTime.Now - _prev) < Interval) 
            {
                PreviousSkipped = true;
                return;
            }

            action();
            PreviousSkipped = false;
            _prev = DateTime.Now;
        }
    }
}
