using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

using Pomodoro.Model;

namespace Pomodoro.Tests.Model
{
    [TestFixture]
    class Clock
    {
        private TimeSpan _testTimeSpan_2ms = TimeSpan.FromMilliseconds(2);

        private IClockDuration _testDuration;

        [SetUp]
        public void SetUp()
        {
            _testDuration = new Pomodoro.Model.ClockDuration(  workDuration: _testTimeSpan_2ms, 
                                                                shortBreak: _testTimeSpan_2ms,
                                                                longBreak: _testTimeSpan_2ms);
        }

        [Test]
        public void DefaultDuration_ShouldBe_DefaultOfClockDuration()
        {
            IClock clock = new Pomodoro.Model.Clock();
            Assert.AreSame(Pomodoro.Model.ClockDuration.Default, clock.Duration);
        }

        [Test]
        public void OverridingDuration_Allowed_DuringConstruction()
        {
            IClockDuration duration = new Pomodoro.Model.ClockDuration(workDuration_minutes: 20, shortBreak_minutes: 4, longBreak_minutes: 10);
            IClock clock = new Pomodoro.Model.Clock(duration);

            Assert.AreSame(duration, clock.Duration);
        }

        [Test]
        public void IdleMode_AtConstruction()
        {
            IClock clock = new Pomodoro.Model.Clock();

            Assert.AreEqual(Mode.Idle, clock.Mode);
        }

        [Test]
        public void WorkMode_OnStartingWork()
        {
            IClock clock = new Pomodoro.Model.Clock();

            clock.StartWork();
            Assert.AreEqual(Mode.Work, clock.Mode);
        }

        [Test]
        public void ShortBreakMode_OnStartingShortBreak()
        {
            IClock clock = new Pomodoro.Model.Clock();

            clock.StartWork();
            clock.StartShortBreak();
            Assert.AreEqual(Mode.ShortBreak, clock.Mode);
        }

        [Test]
        public void LongBreakMode_OnStartingLongBreak()
        {
            IClock clock = new Pomodoro.Model.Clock();

            clock.StartWork();
            clock.StartShortBreak();
            clock.StartLongBreak();

            Assert.AreEqual(Mode.LongBreak, clock.Mode);
        }

        [Test]
        public void IdleMode_OnStoppingFromAnyMode()
        {
            IClock clock = new Pomodoro.Model.Clock();

            clock.StartWork();
            clock.Stop();
            Assert.AreEqual(Mode.Idle, clock.Mode);

            clock.StartShortBreak();
            clock.Stop();
            Assert.AreEqual(Mode.Idle, clock.Mode);

            clock.StartLongBreak();
            clock.Stop();
            Assert.AreEqual(Mode.Idle, clock.Mode);
        }

		#region Work

		[Test]
        public void RaiseWorkStartedEvent_OnStartingWork()
        {
            IClock clock = new Pomodoro.Model.Clock(_testDuration);

            bool eventReceived = false;
            clock.WorkStarted += (sender, e) =>
                {
                    eventReceived = true;
                };

            clock.StartWork();
            Assert.IsTrue(eventReceived);
        }

        [Test]
        public void RaiseWorkingEvent_EveryMillisecond_OnStartingWork()
        {
            int eventCount = 0;

            Action test = () =>
            {
                IClock clock = new Pomodoro.Model.Clock(_testDuration); // WorkDuration = 2ms
                clock.Working += (sender, e) => { eventCount++; };
                clock.StartWork();
            };

            DispatcherHelper.ExecuteOnDispatcherThread(test, millisecondsToWait: 20);
            Assert.AreEqual(2, eventCount);
        }

        [Test]
        public void WorkingEvent_ShouldProvide_ElapsedAndRemainingTime()
        {
            int elapsed_ms = 0;
            int remaining_ms = 0;

            Action test = () =>
            {
                IClock clock = new Pomodoro.Model.Clock(_testDuration); // WorkDuration = 2ms

                bool firstTime = true;
                clock.Working += (sender, e) =>
                {
                    if (firstTime)
                    {
                        firstTime = false;

                        elapsed_ms = e.Elapsed_ms;
                        remaining_ms = e.Remaining_ms;
                    }
                };

                clock.StartWork();
            };

            DispatcherHelper.ExecuteOnDispatcherThread(test, millisecondsToWait: 20);
            Assert.AreEqual(1, elapsed_ms);
            Assert.AreEqual((int)_testDuration.WorkDuration.TotalMilliseconds - elapsed_ms, remaining_ms);
        }


        [Test]
        public void RaiseStoppedEvent_AfterWorkDurationElapsed()
        {
            bool eventRaised = false;

            Action test = () =>
            {
                IClock clock = new Pomodoro.Model.Clock(_testDuration); // WorkDuration = 2ms
                clock.Stopped += (sender, e) => { eventRaised = true; };
                clock.StartWork();
            };

            DispatcherHelper.ExecuteOnDispatcherThread(test, millisecondsToWait: 20);
            Assert.IsTrue(eventRaised);
		}

		#endregion

		#region ShortBreak

		[Test]
        public void RaiseShortBreakStartedEvent_OnStartingShortBreak()
        {
            IClock clock = new Pomodoro.Model.Clock(_testDuration);

            bool eventReceived = false;
            clock.ShortBreakStarted += (sender, e) =>
                {
                    eventReceived = true;
                };

            clock.StartShortBreak();
            Assert.IsTrue(eventReceived);
        }

        [Test]
        public void RaiseShortBreakingEvent_EveryMillisecond_OnStartingShortBreak()
        {
            int eventCount = 0;

            Action test = () =>
            {
                IClock clock = new Pomodoro.Model.Clock(_testDuration); // ShortBreak = 2ms
                clock.ShortBreaking += (sender, e) => { eventCount++; };
                clock.StartShortBreak();
            };

            DispatcherHelper.ExecuteOnDispatcherThread(test, millisecondsToWait: 20);
            Assert.AreEqual(2, eventCount);
        }

        [Test]
        public void ShortBreakingEvent_ShouldProvide_ElapsedAndRemainingTime()
        {
            int elapsed_ms = 0;
            int remaining_ms = 0;

            Action test = () =>
            {
                IClock clock = new Pomodoro.Model.Clock(_testDuration); // ShortBreak = 2ms

                bool firstTime = true;
                clock.ShortBreaking += (sender, e) =>
                {
                    if (firstTime)
                    {
                        firstTime = false;

                        elapsed_ms = e.Elapsed_ms;
                        remaining_ms = e.Remaining_ms;
                    }
                };

                clock.StartShortBreak();
            };

            DispatcherHelper.ExecuteOnDispatcherThread(test, millisecondsToWait: 20);
            Assert.AreEqual(1, elapsed_ms);
            Assert.AreEqual((int)_testDuration.ShortBreak.TotalMilliseconds - elapsed_ms, remaining_ms);
        }


        [Test]
        public void RaiseStoppedEvent_AfterShortBreakElapsed()
        {
            bool eventRaised = false;

            Action test = () =>
            {
                IClock clock = new Pomodoro.Model.Clock(_testDuration); // ShortBreak = 2ms
                clock.Stopped += (sender, e) => { eventRaised = true; };
                clock.StartShortBreak();
            };

            DispatcherHelper.ExecuteOnDispatcherThread(test, millisecondsToWait: 20);
            Assert.IsTrue(eventRaised);
		}

		#endregion

		#region LongBreak

		[Test]
        public void RaiseLongBreakStartedEvent_OnStartingLongBreak()
        {
            IClock clock = new Pomodoro.Model.Clock(_testDuration);

            bool eventReceived = false;
            clock.LongBreakStarted += (sender, e) =>
                {
                    eventReceived = true;
                };

            clock.StartLongBreak();
            Assert.IsTrue(eventReceived);
        }

        [Test]
        public void RaiseLongBreakingEvent_EveryMillisecond_OnStartingLongBreak()
        {
            int eventCount = 0;

            Action test = () =>
            {
                IClock clock = new Pomodoro.Model.Clock(_testDuration); // LongBreak = 2ms
                clock.LongBreaking += (sender, e) => { eventCount++; };
                clock.StartLongBreak();
            };

            DispatcherHelper.ExecuteOnDispatcherThread(test, millisecondsToWait: 20);
            Assert.AreEqual(2, eventCount);
        }

        [Test]
        public void LongBreakingEvent_ShouldProvide_ElapsedAndRemainingTime()
        {
            int elapsed_ms = 0;
            int remaining_ms = 0;

            Action test = () =>
            {
                IClock clock = new Pomodoro.Model.Clock(_testDuration); // LongBreak = 2ms

                bool firstTime = true;
                clock.LongBreaking += (sender, e) =>
                {
                    if (firstTime)
                    {
                        firstTime = false;

                        elapsed_ms = e.Elapsed_ms;
                        remaining_ms = e.Remaining_ms;
                    }
                };

                clock.StartLongBreak();
            };

            DispatcherHelper.ExecuteOnDispatcherThread(test, millisecondsToWait: 20);
            Assert.AreEqual(1, elapsed_ms);
            Assert.AreEqual((int)_testDuration.LongBreak.TotalMilliseconds - elapsed_ms, remaining_ms);
        }

        [Test]
        public void RaiseStoppedEvent_AfterLongBreakElapsed()
        {
            bool eventRaised = false;

            Action test = () =>
            {
                IClock clock = new Pomodoro.Model.Clock(_testDuration); // LongBreak = 2ms
                clock.Stopped += (sender, e) => { eventRaised = true; };
                clock.StartLongBreak();
            };

            DispatcherHelper.ExecuteOnDispatcherThread(test, millisecondsToWait: 20);
            Assert.IsTrue(eventRaised);
		}

		#endregion

        [Test]
        public void Stop_PriorToStartingAgain()
		{
			bool begin = true; // at the beginning, stopped will be false and its okay.
			bool stopped = false;

			bool workStarted = false;
			bool shortBreakStarted = false;
			bool longBreakStarted = false;

			Action test = () =>
			{
				IClock clock = new Pomodoro.Model.Clock(_testDuration);

                clock.Stopped += (sender, e) => { stopped = true; };
				clock.WorkStarted += (sender, e) =>
				{
					if (begin)
						begin = false;
                    else
                        Assert.IsTrue(stopped);

					workStarted = true;
					stopped = false;
				};
				clock.ShortBreakStarted += (sender, e) =>
				{
					if (begin)
						begin = false;
                    else
                        Assert.IsTrue(stopped);

					shortBreakStarted = true;
					stopped = false;
				};
				clock.LongBreakStarted += (sender, e) =>
				{
					if (begin)
						begin = false;
                    else
                        Assert.IsTrue(stopped);

					longBreakStarted = true;
					stopped = false;
				};

				clock.StartWork();
				clock.StartShortBreak();
				clock.StartLongBreak();
				clock.StartWork();
				clock.StartLongBreak();
				clock.StartShortBreak();
				clock.StartWork();
				clock.Stop();
			};

			DispatcherHelper.ExecuteOnDispatcherThread(test, millisecondsToWait: 20);
			Assert.IsTrue(workStarted);
			Assert.IsTrue(shortBreakStarted);
			Assert.IsTrue(longBreakStarted);
        }

        [Test]
        public void Stopping_MultipleTimes_Allowed()
		{
			IClock clock = new Pomodoro.Model.Clock();
			clock.Stop();
			Assert.AreEqual(Mode.Idle, clock.Mode);
			clock.Stop();
			Assert.AreEqual(Mode.Idle, clock.Mode);
			clock.Stop();
			Assert.AreEqual(Mode.Idle, clock.Mode);
        }
	}
}
