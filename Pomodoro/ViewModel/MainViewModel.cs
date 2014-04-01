using System;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using Pomodoro.Model;

namespace Pomodoro.ViewModel
{
	/// <summary>
	/// This class contains properties that the main View can data bind to.
	/// </summary>
	public sealed class MainViewModel : ViewModelBase
	{
		internal IClock Clock { get; set; }
		private Mode _modeBeforeLastStop;
		private uint _pomodorosBeforeLongBreak;

		private TimeSpan _timeRemaining;
		public TimeSpan TimeRemaining
		{
			get { return _timeRemaining; }
			set 
			{ 
				_timeRemaining = value;
				RaisePropertyChanged("TimeRemaining");
			}
		}

		private TimeSpan _currentDuration;
		public TimeSpan CurrentDuration
		{
			get { return _currentDuration; }
			set 
			{ 
				_currentDuration = value;
				RaisePropertyChanged("CurrentDuration");
			}
		}


		public MainViewModel(IClock clock)
		{
			this.Clock = clock;
            this.Clock.WorkStarted += Clock_WorkStarted;
			this.Clock.Working += Clock_Working;
			this.Clock.ShortBreakStarted += Clock_ShortBreakStarted;
			this.Clock.ShortBreaking += Clock_ShortBreaking;
            this.Clock.LongBreakStarted += Clock_LongBreakStarted;
            this.Clock.LongBreaking += Clock_LongBreaking;
			this.Clock.Stopped += Clock_Stopped;

			_modeBeforeLastStop = Mode.Idle;
			_pomodorosBeforeLongBreak = 0;

			this.TimeRemaining = this.Clock.Duration.WorkDuration;
			this.CurrentDuration = this.Clock.Duration.WorkDuration;
		}

		void Clock_Stopped(object sender, System.EventArgs e)
		{
			_modeBeforeLastStop = this.Clock.Mode;

			if (this.Clock.Mode == Mode.Work)
				_pomodorosBeforeLongBreak++;

			this.TimeRemaining = TimeSpan.FromMilliseconds(0);
		}

		private void Clock_LongBreaking(object sender, ClockRunningHandlerArgs e)
		{
			this.TimeRemaining = TimeSpan.FromMilliseconds(e.Remaining_ms);
		}

		private void Clock_LongBreakStarted(object sender, System.EventArgs e)
		{
		}

		void Clock_ShortBreaking(object sender, ClockRunningHandlerArgs e)
		{
			this.TimeRemaining = TimeSpan.FromMilliseconds(e.Remaining_ms);
		}

		void Clock_ShortBreakStarted(object sender, System.EventArgs e)
		{
		}

		void Clock_Working(object sender, ClockRunningHandlerArgs e)
		{
			this.TimeRemaining = TimeSpan.FromMilliseconds(e.Remaining_ms);
		}

		private void Clock_WorkStarted(object sender, System.EventArgs e)
		{
		}

        public Mode Mode
		{
			get { return this.Clock.Mode; }
        }

        public void Start()
		{
			switch (_modeBeforeLastStop)
			{
				case Mode.Idle:
				case Mode.ShortBreak:
				case Mode.LongBreak:
					this.Clock.StartWork();
					break;

				case Mode.Work:
					if (_pomodorosBeforeLongBreak < 4)
					{
						this.Clock.StartShortBreak();
					}
					else
					{
						_pomodorosBeforeLongBreak = 0;
						this.Clock.StartLongBreak();
					}
					break;

				default:
					break;
			}
        }
	}
}