using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Model
{
    public delegate void WorkStartedHandler(object sender, WorkStartedHandlerArgs e);
    public class WorkStartedHandlerArgs : EventArgs {}

    public interface IClock
    {
        IClockDuration Duration { get; set; }
        Mode Mode { get; }

        void StartWork();
        void StartShortBreak();
        void StartLongBreak();
        void Stop();

        event WorkStartedHandler WorkStarted;
    }

    public enum Mode
    {
        Idle = 0,
        Work,
        ShortBreak,
        LongBreak
    }
}
