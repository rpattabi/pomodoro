using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Pomodoro.Model
{
    class HourglassTimer 
    {
        private DispatcherTimer _timer;

        public HourglassTimer(TimeSpan duration, int interval_ms)
        {
            this.Duration = duration;
            this.Interval_ms = interval_ms;

            _timer = new DispatcherTimer(DispatcherPriority.Send)  // Highest priority
            { 
                Interval = TimeSpan.FromMilliseconds(1)  // Tick every millisecond
            };

            _timer.Tick += _timer_Tick;
        }

        public void Start()
        {
            this.Elapsed_ms = 0;
            _timer.Start();
        }

        public void Stop()
        {
            if (!_timer.IsEnabled) return; // Stop only when the timer is running

            _timer.Stop();

            if (Stopped != null)
                Stopped(this, new EventArgs());
        }

        public TimeSpan Duration { get; private set; }
        public int Interval_ms { get; private set; }
        public int Elapsed_ms { get; private set; }

        private void _timer_Tick(object sender, EventArgs e)
        {
            this.Elapsed_ms++;

            if (IsIntervalHit())
            {
                if (Tick != null)
                    Tick(this, new TickEventArgs() { Elapsed_ms = this.Elapsed_ms });
            }

            if (this.Elapsed_ms >= this.Duration.TotalMilliseconds)
            {
                Stop();
            }
        }

        private bool IsIntervalHit()
        {
            return this.Elapsed_ms >= this.Interval_ms
                    && (this.Elapsed_ms % this.Interval_ms == 0);
        }

        public event EventHandler Stopped;
        public event TickHandler Tick;
    }

    delegate void TickHandler(object sender, TickEventArgs e);

    class TickEventArgs : EventArgs
    {
        public int Elapsed_ms { get; set; }
    }
}
