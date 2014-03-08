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
    class Timer
    {
        [Test]
        public void Defaults_ShouldBe_25_5_15()
        {
            ITimerDuration duration = new Pomodoro.Model.TimerDuration();

            Assert.AreEqual(25, duration.WorkDuration.Minutes);
            Assert.AreEqual(5, duration.ShortBreak.Minutes);
            Assert.AreEqual(15, duration.LongBreak.Minutes);
        }

        [Test]
        public void OverridingDefaults_Allowed_AtConstruction()
        {
            ITimerDuration duration = new Pomodoro.Model.TimerDuration(workDuration: 20,
                                                    shortBreak: 4,
                                                    longBreak: 25);

            Assert.AreEqual(20, duration.WorkDuration.Minutes);
            Assert.AreEqual(4, duration.ShortBreak.Minutes);
            Assert.AreEqual(25, duration.LongBreak.Minutes);
        }

        [Test]
        public void RevisingAnyDuration_Allowed()
        {
            ITimerDuration duration = new Pomodoro.Model.TimerDuration();

            duration.WorkDuration = new TimeSpan(hours: 0, minutes: 30, seconds: 0);
            Assert.AreEqual(30, duration.WorkDuration.Minutes);

            duration.ShortBreak = new TimeSpan(hours: 0, minutes: 10, seconds: 0);
            Assert.AreEqual(10, duration.ShortBreak.Minutes);

            duration.LongBreak = new TimeSpan(hours: 0, minutes: 20, seconds: 0);
            Assert.AreEqual(20, duration.LongBreak.Minutes);
        }

        [Test]
        public void DefaultInstance_ShouldHave_25_5_15()
        {
            ITimerDuration defaultDuration = Pomodoro.Model.TimerDuration.Default;

            Assert.AreEqual(25, defaultDuration.WorkDuration.Minutes);
            Assert.AreEqual(5, defaultDuration.ShortBreak.Minutes);
            Assert.AreEqual(15, defaultDuration.LongBreak.Minutes);
        }
    }
}
