using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Model
{
    public class Clock : IClock
    {
        private HourglassTimer _runningHourglassTimer;

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
            SetupHourglassTimer(this.Duration.WorkDuration);

            this.Mode = Mode.Work;

            if (WorkStarted != null)
                WorkStarted(this, new WorkStartedHandlerArgs());

            _runningHourglassTimer.Start();
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
            EndHourglassTimer();
            this.Mode = Mode.Idle;

            if (Stopped != null)
                Stopped(this, new EventArgs());
        }

        private void SetupHourglassTimer(TimeSpan duration)
        {
            if (_runningHourglassTimer != null)
                EndHourglassTimer();

            _runningHourglassTimer = new HourglassTimer(
                                            duration: duration,
                                            interval_ms: 1);

            _runningHourglassTimer.Tick += _hourglassTimer_Tick;
            _runningHourglassTimer.Stopped += _hourglassTimer_Stopped;
        }

        private void EndHourglassTimer()
        {
            if (_runningHourglassTimer != null)
            {
                _runningHourglassTimer.Stop();
                _runningHourglassTimer = null;
            }
        }

        void _hourglassTimer_Tick(object sender, TickEventArgs e)
        {
            switch (Mode)
            {
                case Mode.Idle:
                    break;
                case Mode.Work:
                    if (Working == null) break;

                    var clockingRunningEventArgs = new ClockRunningHandlerArgs()
                    {
                        Elapsed_ms = e.Elapsed_ms,
                        Remaining_ms = (int)this.Duration.WorkDuration.TotalMilliseconds - e.Elapsed_ms
                    };

                    Working(this, clockingRunningEventArgs);
                    break;
                case Mode.ShortBreak:
                    break;
                case Mode.LongBreak:
                    break;
                default:
                    break;
            }
        }

        void _hourglassTimer_Stopped(object sender, EventArgs e)
        {
            Stop();
        }

        public event WorkStartedHandler WorkStarted;
        public event ClockRunningHandler Working;
        public event EventHandler Stopped;
    }
}
