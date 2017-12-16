using System;
using System.Net;
using System.Threading;
using AddressCacheProject;
using NUnit.Framework;

namespace AddressCacheTest.BaseTests
{
    [TestFixture]
    public class TakeTests
    {
        [Test]
        public void TestTake()
        {
            var addressCache = new AddressCache(new TimeSpan(0, 0, 2));

            Assert.True(addressCache.Add(IPAddress.Parse("1.1.1.1")));
            Assert.AreEqual("1.1.1.1", addressCache.Take().ToString());

            Assert.True(addressCache.Add(IPAddress.Parse("1.1.1.1")));
            Assert.True(addressCache.Add(IPAddress.Parse("2.2.2.2")));
            Assert.True(addressCache.Add(IPAddress.Parse("3.3.3.3")));

            Assert.AreEqual("3.3.3.3", addressCache.Take().ToString());
            Assert.AreEqual("2.2.2.2", addressCache.Take().ToString());
            Assert.AreEqual("1.1.1.1", addressCache.Take().ToString());
            Assert.Null(addressCache.Peek());
        }

        [Test]
        public void TestRemove()
        {
            var addressCache = new AddressCache(new TimeSpan(0, 0, 2));

            Assert.True(addressCache.Add(IPAddress.Parse("1.1.1.1")));
            Assert.True(addressCache.Add(IPAddress.Parse("2.2.2.2")));
            Assert.True(addressCache.Add(IPAddress.Parse("3.3.3.3")));
            Assert.True(addressCache.Add(IPAddress.Parse("4.4.4.4")));

            Assert.True(addressCache.Remove(IPAddress.Parse("4.4.4.4")));
            Assert.True(addressCache.Remove(IPAddress.Parse("2.2.2.2")));

            Assert.AreEqual("3.3.3.3", addressCache.Take().ToString());
            Assert.AreEqual("1.1.1.1", addressCache.Take().ToString());
        }

        [Test]
        public void TestExpired()
        {
            var addressCache = new AddressCache(new TimeSpan(0, 0, 2));
            Assert.True(addressCache.Add(IPAddress.Parse("1.1.1.1")));
            Assert.True(addressCache.Add(IPAddress.Parse("2.2.2.2")));
            Thread.Sleep(1000);
            Assert.True(addressCache.Add(IPAddress.Parse("3.3.3.3")));
            Assert.True(addressCache.Add(IPAddress.Parse("4.4.4.4")));
            Thread.Sleep(1000);
            
            Assert.AreEqual("4.4.4.4", addressCache.Take().ToString());
            Assert.AreEqual("3.3.3.3", addressCache.Take().ToString());
            Assert.Null(addressCache.Peek());
        }

        [Test]
        public void TestRemoveExpired()
        {
            var addressCache = new AddressCache(new TimeSpan(0, 0, 2));
            Assert.True(addressCache.Add(IPAddress.Parse("1.1.1.1")));
            Assert.True(addressCache.Add(IPAddress.Parse("2.2.2.2")));
            Thread.Sleep(1000);
            Assert.True(addressCache.Add(IPAddress.Parse("3.3.3.3")));
            Assert.True(addressCache.Add(IPAddress.Parse("4.4.4.4")));
            Thread.Sleep(1000);
            Assert.True(addressCache.Remove(IPAddress.Parse("4.4.4.4")));

            Assert.AreEqual("3.3.3.3", addressCache.Take().ToString());
        }
    }
}