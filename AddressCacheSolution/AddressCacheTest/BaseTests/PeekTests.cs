using System;
using System.Net;
using System.Threading;
using AddressCacheProject;
using NUnit.Framework;

namespace AddressCacheTest.BaseTests
{
    [TestFixture]
    public class PeekTests
    {
        [Test]
        public void TestPeek()
        {
            var addressCache = new AddressCache(new TimeSpan(0, 0, 10));
            Assert.Null(addressCache.Peek());

            Assert.True(addressCache.Add(IPAddress.Parse("1.1.1.1")));
            Assert.AreEqual("1.1.1.1", addressCache.Peek().ToString());

            Assert.True(addressCache.Add(IPAddress.Parse("2.2.2.2")));
            Assert.AreEqual("2.2.2.2", addressCache.Peek().ToString());
        }

        [Test]
        public void TestExpiredPeek()
        {
            var addressCache = new AddressCache(new TimeSpan(0, 0, 2));
            Assert.True(addressCache.Add(IPAddress.Parse("1.1.1.1")));
            Assert.AreEqual("1.1.1.1", addressCache.Peek().ToString());

            Thread.Sleep(2000);
            Assert.Null(addressCache.Peek());

            Assert.True(addressCache.Add(IPAddress.Parse("1.1.1.1")));
            Thread.Sleep(1000);
            Assert.True(addressCache.Add(IPAddress.Parse("2.2.2.2")));
            Thread.Sleep(1000);
            Assert.AreEqual("2.2.2.2", addressCache.Peek().ToString());
            Assert.AreEqual("2.2.2.2", addressCache.Peek().ToString());
            Thread.Sleep(1000);
            Assert.Null(addressCache.Peek());
        }

        [Test]
        public void TestStressPeak()
        {
            var addressCache = new AddressCache(new TimeSpan(0, 0, 2));
            Assert.True(addressCache.Add(IPAddress.Parse("1.1.1.1")));
            Assert.True(addressCache.Add(IPAddress.Parse("2.2.2.2")));
            Assert.True(addressCache.Add(IPAddress.Parse("3.3.3.3")));
            Assert.True(addressCache.Add(IPAddress.Parse("4.4.4.4")));
            Assert.True(addressCache.Add(IPAddress.Parse("5.5.5.5")));

            Assert.AreEqual("5.5.5.5", addressCache.Peek().ToString());
            Thread.Sleep(1000);

            Assert.True(addressCache.Add(IPAddress.Parse("6.6.6.6")));

            Assert.AreEqual("6.6.6.6", addressCache.Peek().ToString());
            
            Thread.Sleep(1000);
            Assert.True(addressCache.Add(IPAddress.Parse("5.5.5.5")));

            Assert.AreEqual("5.5.5.5", addressCache.Peek().ToString());
            
            Thread.Sleep(2000);

            Assert.Null(addressCache.Peek());
        }
        
        [Test]
        public void TestRemove()
        {
            var addressCache = new AddressCache(new TimeSpan(0, 0, 2));
            Assert.True(addressCache.Add(IPAddress.Parse("1.1.1.1")));
            Assert.True(addressCache.Add(IPAddress.Parse("2.2.2.2")));
            Assert.AreEqual("2.2.2.2", addressCache.Peek().ToString());

            Assert.True(addressCache.Remove(IPAddress.Parse("2.2.2.2")));
            Assert.AreEqual("1.1.1.1", addressCache.Peek().ToString());
            
            Assert.True(addressCache.Remove(IPAddress.Parse("1.1.1.1")));
            Assert.Null(addressCache.Peek());
        }
    }
}