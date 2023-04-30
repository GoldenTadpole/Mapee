using System;
using System.Threading;

namespace Mapper.Gui
{
    public static class BackgroundWork
    {
        public static void Run(TimeSpan delay, Action action) 
        {
            new Thread(() =>
            {
                while (true) 
                {
                    action();
                    Thread.Sleep(delay);
                }
            })
            {
                IsBackground = true
            }.Start();
        }
    }
}
