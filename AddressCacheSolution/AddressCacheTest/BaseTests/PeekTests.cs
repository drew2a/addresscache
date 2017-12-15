using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using AddressCacheProject;
using NUnit.Framework;

namespace AddressCacheTest
{
    [TestFixture]
    public class PeekTests
    {
        [Test]
        public void TestAdd()
        {
            var addressCache = new AddressCache(new TimeSpan(0, 0, 10));
            Assert.Null(addressCache.Peek());

            Assert.True(addressCache.Add(new Uri("http://some.url")));
            Assert.AreEqual("http://some.url/", addressCache.Peek().AbsoluteUri);

            Assert.True(addressCache.Add(new Uri("http://agoda.com")));
            Assert.AreEqual("http://agoda.com/", addressCache.Peek().AbsoluteUri);
        }

        [Test]
        public void TestExpiredAdd()
        {
            var addressCache = new AddressCache(new TimeSpan(0, 0, 2));
            Assert.True(addressCache.Add(new Uri("http://some.url")));
            Assert.AreEqual("http://some.url/", addressCache.Peek().AbsoluteUri);

            Thread.Sleep(2000);
            Assert.Null(addressCache.Peek());

            Assert.True(addressCache.Add(new Uri("http://some.url")));
            Thread.Sleep(1000);
            Assert.True(addressCache.Add(new Uri("http://agoda.com")));
            Thread.Sleep(1000);
            Assert.AreEqual("http://agoda.com/", addressCache.Peek().AbsoluteUri);
            Thread.Sleep(1000);
            Assert.Null(addressCache.Peek());
        }
    }
}