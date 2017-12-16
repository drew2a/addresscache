using System;
using NUnit.Framework;

namespace AddressCacheTest.MultiThreadTests
{
    [TestFixture]
    [Category("LongRunning")]
    public class MonkeyTests
    {
        [Test]
        public void RegularMonkeys()
        {
            var monkeys = new Monkeys(new TimeSpan(0, 0, 1), 100, 20, 200, 50);
            monkeys.ReleaseTheMonkeys();
            Assert.Pass();
        }

        [Test]
        public void SpeedyMonkeys()
        {
            var monkeys = new Monkeys(new TimeSpan(0, 0, 1), 200, 200, 0, 0);
            monkeys.ReleaseTheMonkeys();
            Assert.Pass();
        }

        [Test]
        public void BigPrideOfSpeedyMonkeys()
        {
            var monkeys = new Monkeys(new TimeSpan(0, 0, 3), 1000, 1000, 0, 0);
            monkeys.ReleaseTheMonkeys();
            Assert.Pass();
        }

        [Test]
        public void IndustriousMonkeys()
        {
            var monkeys = new Monkeys(new TimeSpan(0, 0, 3), 100, 10000, 0, 1);
            monkeys.ReleaseTheMonkeys();
            Assert.Pass();
        }
    }
}