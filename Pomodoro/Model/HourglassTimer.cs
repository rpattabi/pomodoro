﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Pomodoro.Model
{
    class HourglassTimer : IDisposable
    {
        private System.Threading.Timer _timer;
        private Dispatcher _dispatcher;

        public HourglassTimer(TimeSpan duration, int interval_ms)
        {
            this.Duration = duration;
            this.Interval_ms = interval_ms;

            _timer = new Timer( OnTick, state: null, 
                                dueTime: Timeout.Infinite, period: Timeout.Infinite);

            //_dispatcher = Dispatcher.CurrentDispatcher;
        }

        public void Start()
        {
            this.Elapsed_ms = 0;
            _timer.Change(dueTime: 0, period: 1); // starts timer. signals every millisecond
        }

        public void Stop()
        {
            _timer.Change(dueTime: Timeout.Infinite, period: Timeout.Infinite); // stops timer

            if (Stopped != null)
                Stopped(this);
        }

        public TimeSpan Duration { get; private set; }
        public int Interval_ms { get; private set; }
        public int Elapsed_ms { get; private set; }

        private void OnTick(object state)
        {
            //_dispatcher.BeginInvoke(new Action(()=>
            //    {
                    this.Elapsed_ms++;

                    if (IsIntervalHit())
                    {
                        if (Tick != null)
                            Tick(this, new TickEventArgs() { Elapsed_ms = this.Elapsed_ms });
                    }

                    if (this.Elapsed_ms == this.Duration.TotalMilliseconds)
                    {
                        Stop();
                    }
                //}));
        }

        private bool IsIntervalHit()
        {
            return this.Elapsed_ms >= this.Interval_ms
                    && (this.Elapsed_ms % this.Interval_ms == 0);
        }

        public event StoppedHandler Stopped;
        public event TickHandler Tick;

        #region Disposable Pattern

        //
        // Reference: http://msdn.microsoft.com/en-us/library/ms244737.aspx
        //

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //// NOTE: Leave out the finalizer altogether if this class doesn't 
        //// own unmanaged resources itself, but leave the other methods
        //// exactly as they are. 
        //~HourglassTimer()
        //{
        //    // Finalizer calls Dispose(false)
        //    Dispose(false);
        //}

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (_timer != null)
                {
                    _timer.Dispose();
                    _timer = null;
                }
            }

            // free native resources if there are any.
            // here
        }

        #endregion
    }

    delegate void StoppedHandler(object sender);
    delegate void TickHandler(object sender, TickEventArgs e);

    class TickEventArgs : EventArgs
    {
        public int Elapsed_ms { get; set; }
    }
}