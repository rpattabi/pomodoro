using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Model
{
    public delegate void WorkStartedHandler(object sender, WorkStartedHandlerArgs e);
    public class WorkStartedHandlerArgs : EventArgs {}

    public delegate void ClockRunningHandler(object sender, ClockRunningHandlerArgs e);
    public class ClockRunningHandlerArgs : EventArgs 
    {
        int Elapsed_ms { get; set; }
        int Remaining_ms { get; set; }
    }

    public interface IClock
    {
        IClockDuration Duration { get; set; }
        Mode Mode { get; }

        void StartWork();
        void StartShortBreak();
        void StartLongBreak();
        void Stop();

        event WorkStartedHandler WorkStarted;
        event ClockRunningHandler Working;
    }

    public enum Mode
    {
        Idle = 0,
        Work,
        ShortBreak,
        LongBreak
    }
}
