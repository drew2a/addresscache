using System;
using System.Net;
using System.Threading;
using AddressCacheProject;
using NUnit.Framework;

namespace AddressCacheTest.BaseTests
{
    [TestFixture]
    public class RemoveTests
    {
        [Test]
        public void Remove()
        {
            var addressCache = new AddressCache(new TimeSpan(0, 0, 2));
            Assert.True(addressCache.Add(IPAddress.Parse("1.1.1.1")));

            Assert.False(addressCache.Remove(IPAddress.Parse("2.2.2.2")));
            Assert.True(addressCache.Remove(IPAddress.Parse("1.1.1.1")));
        }

        [Test]
        public void ExpiredRemove()
        {
            var addressCache = new AddressCache(new TimeSpan(0, 0, 2));
            Assert.True(addressCache.Add(IPAddress.Parse("1.1.1.1")));
            Thread.Sleep(2000);

            Assert.False(addressCache.Remove(IPAddress.Parse("1.1.1.1")));
        }
    }
}