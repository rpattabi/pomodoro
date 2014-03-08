using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Model
{
    public class Clock : IClock
    {
        public Clock()
            : this(ClockDuration.Default)
        {
        }

        public Clock(IClockDuration duration)
        {
            this.Duration = duration;
            this.Mode = Mode.Idle;
        }

        public IClockDuration Duration { get; set; }
        public Mode Mode { get; private set; }

        public void StartWork()
        {
            this.Mode = Mode.Work;
        }

        public void StartShortBreak()
        {
            this.Mode = Mode.ShortBreak;
        }

        public void StartLongBreak()
        {
            this.Mode = Mode.LongBreak;
        }

        public void Stop()
        {
            this.Mode = Mode.Idle;
        }
    }
}
