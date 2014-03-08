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
        public int Elapsed_ms { get; set; }
        public int Remaining_ms { get; set; }
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
        event EventHandler Stopped;
    }

    public enum Mode
    {
        Idle = 0,
        Work,
        ShortBreak,
        LongBreak
    }
}
