using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Pomodoro.Model;

namespace Pomodoro.Tests.ViewModel
{
    [TestFixture]
	class MainViewModel
	{
        private IClock _testClock;
        private IClockDuration _testDuration;
        private TimeSpan _testTimeSpan_2ms = TimeSpan.FromMilliseconds(2);

        [SetUp]
        public void SetUp()
		{
            _testDuration = new ClockDuration(  workDuration: _testTimeSpan_2ms, 
                                                shortBreak: _testTimeSpan_2ms,
                                                longBreak: _testTimeSpan_2ms);
            _testClock = new Clock(_testDuration);
        }

        [Test]
        public void StartingOnIdle_ShouldStartWorkingMode()
		{
			var vm = new Pomodoro.ViewModel.MainViewModel(_testClock);
			vm.Start(); 

			Assert.AreEqual(Mode.Work, vm.Mode); // Expecting clock to be in working mode still
			vm.Clock.Stop();
        }

        [Test]
        public void StartingAfterWorking_ShouldStartShortBreak()
		{
			var vm = new Pomodoro.ViewModel.MainViewModel(_testClock);
			vm.Start(); while (vm.Mode == Mode.Work);
			vm.Start();

			Assert.AreEqual(Mode.ShortBreak, vm.Mode); // Expecting clock to be in working mode still
			vm.Clock.Stop();
        }
	}
}
