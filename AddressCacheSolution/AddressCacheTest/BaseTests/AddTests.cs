using System;
using System.Threading;
using AddressCacheProject;
using NUnit.Framework;

namespace AddressCacheTest
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
            var addressCache = new AddressCache(new TimeSpan(0, 0, 2));
            Assert.True(addressCache.Add(new Uri("http://some.url")));
            Assert.True(addressCache.Add(new Uri("http://agoda.com")));
            Assert.False(addressCache.Add(new Uri("http://some.url")));
            Thread.Sleep(2000);
            Assert.True(addressCache.Add(new Uri("http://some.url")));
            Assert.AreEqual(2, addressCache.Count());
        }
    }
}