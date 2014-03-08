using System;
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
        private TimeSpan _testDuration_2ms = new TimeSpan(  days: 0, hours: 0, minutes: 0, seconds: 0, 
                                                        milliseconds: 2);

        [SetUp]
        public void SetUp()
        {
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
            IClockDuration duration = new ClockDuration(_testDuration_2ms,
                                                        _testDuration_2ms,
                                                        _testDuration_2ms);
            IClock clock = new Pomodoro.Model.Clock(duration);

            bool eventReceived = false;
            clock.WorkStarted += (sender, e) =>
                {
                    eventReceived = true;
                };

            clock.StartWork();
            Assert.IsTrue(eventReceived);
        }
    }
}
