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
        [Test]
        public void DefaultDuration_ShouldBe_DefaultOfClockDuration()
        {
            IClock clock = new Pomodoro.Model.Clock();
            Assert.AreSame(ClockDuration.Default, clock.Duration);
        }

        [Test]
        public void OverridingDuration_Allowed_DuringConstruction()
        {
            IClockDuration duration = new ClockDuration(workDuration: 20, shortBreak: 4, longBreak: 10);
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
    }
}
