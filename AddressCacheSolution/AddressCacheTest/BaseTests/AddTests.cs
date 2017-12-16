using System;
using System.Threading;
using AddressCacheProject;
using NUnit.Framework;

namespace AddressCacheTest.BaseTests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void TestAdd()
        {
            var addressCache = new AddressCache(new TimeSpan(0, 0, 10));
            Assert.True(addressCache.Add(new Uri("http://some.url")));
            Assert.False(addressCache.Add(new Uri("http://some.url")));
            Assert.False(addressCache.Add(null));
            Assert.AreEqual(1, addressCache.Count());
        }

        [Test]
        public void TestExpiredAdd()
        {
            var timeSpan = new TimeSpan(0, 0, 2);
            var addressCache = new AddressCache(timeSpan);
            Assert.True(addressCache.Add(new Uri("http://some.url")));
            Assert.True(addressCache.Add(new Uri("http://agoda.com")));
            Assert.False(addressCache.Add(new Uri("http://some.url")));
            Assert.AreEqual(2, addressCache.Count());

            Thread.Sleep(timeSpan);
            Assert.AreEqual(0, addressCache.Count());

            Assert.True(addressCache.Add(new Uri("http://some.url")));
            Assert.AreEqual(1, addressCache.Count());
        }
    }
}