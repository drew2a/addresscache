using System;
using System.Net;
using System.Threading;
using AddressCacheProject;
using NUnit.Framework;

namespace AddressCacheTest.BaseTests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Add()
        {
            var addressCache = new AddressCache(new TimeSpan(0, 0, 10));
            Assert.True(addressCache.Add(IPAddress.Parse("1.1.1.1")));
            Assert.False(addressCache.Add(IPAddress.Parse("1.1.1.1")));
            Assert.False(addressCache.Add(null));
        }

        [Test]
        public void ExpiredAdd()
        {
            var addressCache = new AddressCache(new TimeSpan(0, 0, 2));
            Assert.True(addressCache.Add(IPAddress.Parse("1.1.1.1")));
            Assert.True(addressCache.Add(IPAddress.Parse("2.2.2.2")));
            Assert.False(addressCache.Add(IPAddress.Parse("1.1.1.1")));
            Assert.False(addressCache.Add(IPAddress.Parse("2.2.2.2")));

            Thread.Sleep(2000);
            Assert.True(addressCache.Add(IPAddress.Parse("1.1.1.1")));
            Assert.True(addressCache.Add(IPAddress.Parse("2.2.2.2")));
        }
    }
}