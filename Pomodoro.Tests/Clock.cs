﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

using Pomodoro.Model;

namespace Pomodoro.Tests
{
    [TestFixture]
    class Clock
    {
        private TimeSpan _testTimeSpan_2ms = TimeSpan.FromMilliseconds(2);

        private IClockDuration _testDuration;

        [SetUp]
        public void SetUp()
        {
            _testDuration = new ClockDuration(  workDuration: _testTimeSpan_2ms, 
                                                shortBreak: _testTimeSpan_2ms,
                                                longBreak: _testTimeSpan_2ms);
        }

        [Test]
        public void DefaultDuration_ShouldBe_DefaultOfClockDuration()
        {
            IClock clock = new Pomodoro.Model.Clock();
            Assert.AreSame(ClockDuration.Default, clock.Duration);
        }

        [Test]
        public void OverridingDuration_Allowed_DuringConstruction()
        {
            IClockDuration duration = new ClockDuration(workDuration_minutes: 20, shortBreak_minutes: 4, longBreak_minutes: 10);
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
    }
}
