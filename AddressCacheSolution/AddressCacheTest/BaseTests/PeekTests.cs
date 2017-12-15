using System;
using System.Threading;
using AddressCacheProject;
using NUnit.Framework;

namespace AddressCacheTest
{
    [TestFixture]
    public class PeekTests
    {
        [Test]
        public void TestPeek()
        {
            var addressCache = new AddressCache(new TimeSpan(0, 0, 10));
            Assert.Null(addressCache.Peek());

            Assert.True(addressCache.Add(new Uri("http://some.url")));
            Assert.AreEqual("http://some.url/", addressCache.Peek().AbsoluteUri);

            Assert.True(addressCache.Add(new Uri("http://agoda.com")));
            Assert.AreEqual("http://agoda.com/", addressCache.Peek().AbsoluteUri);
        }

        [Test]
        public void TestExpiredPeek()
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
            Assert.AreEqual("http://agoda.com/", addressCache.Peek().AbsoluteUri);
            Thread.Sleep(1000);
            Assert.Null(addressCache.Peek());
        }

        [Test]
        public void TestStressPeak()
        {
            var addressCache = new AddressCache(new TimeSpan(0, 0, 2));
            Assert.True(addressCache.Add(new Uri("http://a.a")));
            Assert.True(addressCache.Add(new Uri("http://b.b")));
            Assert.True(addressCache.Add(new Uri("http://c.c")));
            Assert.True(addressCache.Add(new Uri("http://d.d")));
            Assert.True(addressCache.Add(new Uri("http://e.e")));
            Assert.AreEqual(5, addressCache.Count());
            Assert.AreEqual(5, addressCache.HistoryCount());

            Assert.AreEqual("http://e.e/", addressCache.Peek().AbsoluteUri);
            Thread.Sleep(1000);

            Assert.True(addressCache.Add(new Uri("http://f.f")));
            Assert.AreEqual(6, addressCache.Count());
            Assert.AreEqual(6, addressCache.HistoryCount());

            Assert.AreEqual("http://f.f/", addressCache.Peek().AbsoluteUri);
            
            Thread.Sleep(1000);
            Assert.AreEqual(1, addressCache.Count());
            Assert.AreEqual(6, addressCache.HistoryCount());
            Assert.True(addressCache.Add(new Uri("http://e.e")));
            Assert.AreEqual(2, addressCache.HistoryCount());

            Assert.AreEqual("http://e.e/", addressCache.Peek().AbsoluteUri);
            
            Thread.Sleep(2000);
            Assert.AreEqual(0, addressCache.Count());
            Assert.AreEqual(2, addressCache.HistoryCount());

            Assert.Null(addressCache.Peek());
        }
    }
}