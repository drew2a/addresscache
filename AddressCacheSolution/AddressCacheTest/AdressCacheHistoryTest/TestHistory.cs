﻿using System;
using System.Threading;
using AddressCacheProject;
using NUnit.Framework;

namespace AddressCacheTest.AdressCacheHistoryTest
{
    [TestFixture]
    public class TestHistory
    {
        [Test]
        public void TestExpired()
        {
            var addressCache = new AddressCache(new TimeSpan(0, 0, 2));
            Assert.True(addressCache.Add(new Uri("http://a.a")));
            Assert.True(addressCache.Add(new Uri("http://b.b")));

            Assert.AreEqual(2, addressCache.Count());
            Assert.AreEqual(2, addressCache.HistoryCount());

            Thread.Sleep(2000);

            Assert.AreEqual(0, addressCache.Count());
            Assert.AreEqual(2, addressCache.HistoryCount());

            Assert.True(addressCache.Add(new Uri("http://c.c")));
            Assert.AreEqual(1, addressCache.Count());
            Assert.AreEqual(1, addressCache.HistoryCount());

            Assert.AreEqual("http://c.c/", addressCache.Take().AbsoluteUri);
        }

        [Test]
        public void TestRemove()
        {
            var addressCache = new AddressCache(new TimeSpan(0, 0, 2));
            Assert.True(addressCache.Add(new Uri("http://a.a")));
            Assert.True(addressCache.Add(new Uri("http://b.b")));
            Assert.AreEqual("http://b.b/", addressCache.Peek().AbsoluteUri);

            Assert.True(addressCache.Remove(new Uri("http://b.b")));
            Assert.AreEqual("http://a.a/", addressCache.Peek().AbsoluteUri);

            Assert.True(addressCache.Remove(new Uri("http://a.a")));
            Assert.Null(addressCache.Peek());
        }

        [Test]
        public void TestTrickyRemove()
        {
            var addressCache = new AddressCache(new TimeSpan(0, 0, 2));

            Assert.True(addressCache.Add(new Uri("http://a.a")));
            Assert.True(addressCache.Add(new Uri("http://b.b")));
            Assert.True(addressCache.Add(new Uri("http://c.c")));
            Assert.True(addressCache.Add(new Uri("http://d.d")));
            Assert.True(addressCache.Add(new Uri("http://e.e")));
            
            Assert.True(addressCache.Remove(new Uri("http://a.a")));
            Assert.True(addressCache.Remove(new Uri("http://c.c")));
            Assert.True(addressCache.Remove(new Uri("http://e.e")));
            
            Assert.AreEqual("http://d.d/", addressCache.Peek().AbsoluteUri);

            Assert.True(addressCache.Add(new Uri("http://f.f")));
            
            Assert.AreEqual(3, addressCache.HistoryCount());
        }
    }
}