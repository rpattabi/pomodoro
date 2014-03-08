using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Model
{
    public interface IClock
    {
        ITimerDuration Duration { get; set; }
        Mode Mode { get; }

        void StartWork();
        void StartShortBreak();
        void StartLongBreak();

        void Stop();
    }

    public enum Mode
    {
        Idle = 0,
        Work,
        ShortBreak,
        LongBreak
    }
}
