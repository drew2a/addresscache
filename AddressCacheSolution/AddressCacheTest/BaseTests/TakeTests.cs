using System;
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
            Assert.Null(addressCache.Take());

            Assert.True(addressCache.Add(new Uri("http://some.url")));
            Assert.AreEqual("http://some.url/", addressCache.Take().AbsoluteUri);

            Assert.AreEqual(0, addressCache.Count());
            Assert.Null(addressCache.Take());

            Assert.True(addressCache.Add(new Uri("http://a.a")));
            Assert.True(addressCache.Add(new Uri("http://b.b")));
            Assert.True(addressCache.Add(new Uri("http://c.c")));

            Assert.AreEqual("http://c.c/", addressCache.Take().AbsoluteUri);
            Assert.AreEqual("http://b.b/", addressCache.Take().AbsoluteUri);
            Assert.AreEqual("http://a.a/", addressCache.Take().AbsoluteUri);
            Assert.Null(addressCache.Take());
            Assert.AreEqual(0, addressCache.Count());
        }

        [Test]
        public void TestRemove()
        {
            var addressCache = new AddressCache(new TimeSpan(0, 0, 2));

            Assert.True(addressCache.Add(new Uri("http://a.a")));
            Assert.True(addressCache.Add(new Uri("http://b.b")));
            Assert.True(addressCache.Add(new Uri("http://c.c")));
            Assert.True(addressCache.Add(new Uri("http://d.d")));

            Assert.True(addressCache.Remove(new Uri("http://d.d")));
            Assert.True(addressCache.Remove(new Uri("http://b.b")));

            Assert.AreEqual("http://c.c/", addressCache.Take().AbsoluteUri);
            Assert.AreEqual("http://a.a/", addressCache.Take().AbsoluteUri);
            Assert.Null(addressCache.Take());
        }

        [Test]
        public void TestExpired()
        {
            var addressCache = new AddressCache(new TimeSpan(0, 0, 2));
            Assert.True(addressCache.Add(new Uri("http://a.a")));
            Assert.True(addressCache.Add(new Uri("http://b.b")));
            Thread.Sleep(1000);
            Assert.True(addressCache.Add(new Uri("http://c.c")));
            Assert.True(addressCache.Add(new Uri("http://d.d")));
            Thread.Sleep(1000);
            
            Assert.AreEqual("http://d.d/", addressCache.Take().AbsoluteUri);
            Assert.AreEqual("http://c.c/", addressCache.Take().AbsoluteUri);
            Assert.Null(addressCache.Take());
        }

        [Test]
        public void TestRemoveExpired()
        {
            var addressCache = new AddressCache(new TimeSpan(0, 0, 2));
            Assert.True(addressCache.Add(new Uri("http://a.a")));
            Assert.True(addressCache.Add(new Uri("http://b.b")));
            Thread.Sleep(1000);
            Assert.True(addressCache.Add(new Uri("http://c.c")));
            Assert.True(addressCache.Add(new Uri("http://d.d")));
            Thread.Sleep(1000);
            Assert.True(addressCache.Remove(new Uri("http://d.d")));

            Assert.AreEqual("http://c.c/", addressCache.Take().AbsoluteUri);
            Assert.Null(addressCache.Take());
        }
    }
}