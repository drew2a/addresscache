using NUnit.Framework;

namespace AddressCacheTest.MultiThreadTests
{
    [TestFixture]
    public class MonkeyTests
    {
        [Test]
        public void RegularMonkeys()
        {
            var monkeys = new Monkeys(100, 100, 100, 100);
            monkeys.ReleaseTheMonkeys();
            Assert.Pass();
        }

        [Test]
        public void SpeedyMonkeys()
        {
            var monkeys = new Monkeys(200, 200, 0, 0);
            monkeys.ReleaseTheMonkeys();
            Assert.Pass();
        }
    }
}