using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PomodoroTests
{
    [TestFixture]
    public class NUnitPractice
    {
        [Test]
        public void SumOfTwoNumbers()
        {
            Assert.AreEqual(10, 5 + 5);
        }

        [Test]
        public void SumOfTwoNumbers2()
        {
            Assert.AreEqual(10, 5 + 4);
        }
    }
}
