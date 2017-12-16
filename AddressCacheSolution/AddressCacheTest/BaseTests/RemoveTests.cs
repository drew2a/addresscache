using System;
using System.Threading;
using AddressCacheProject;
using NUnit.Framework;

namespace AddressCacheTest.BaseTests
{
    [TestFixture]
    public class RemoveTests
    {
        [Test]
        public void TestRemove()
        {
            var addressCache = new AddressCache(new TimeSpan(0, 0, 2));
            Assert.True(addressCache.Add(new Uri("http://some.url")));

            Assert.False(addressCache.Remove(new Uri("http://agoda.com")));
            Assert.True(addressCache.Remove(new Uri("http://some.url")));
            
            Assert.AreEqual(0, addressCache.Count());
        }

        [Test]
        public void TestExpiredRemove()
        {
            var addressCache = new AddressCache(new TimeSpan(0, 0, 2));
            Assert.True(addressCache.Add(new Uri("http://some.url")));
            Thread.Sleep(2000);

            Assert.False(addressCache.Remove(new Uri("http://some.url")));
            Assert.AreEqual(0, addressCache.Count());
        }
    }
}