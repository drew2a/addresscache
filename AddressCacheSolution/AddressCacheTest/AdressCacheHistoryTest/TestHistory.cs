using System;
using System.Runtime.Caching;
using AddressCacheProject;
using NUnit.Framework;

namespace AddressCacheTest.AdressCacheHistoryTest
{
    [TestFixture]
    public class TestHistory
    {
        [Test]
        public void TestRemove()
        {
            var memoryCache = new MemoryCache("MemoryCache")
            {
                {"1", "1", DateTimeOffset.Now.Add(new TimeSpan(1, 1, 1))},
                {"2", "2", DateTimeOffset.Now.Add(new TimeSpan(1, 1, 1))},
                {"3", "3", DateTimeOffset.Now.Add(new TimeSpan(1, 1, 1))}
            };


            var addressCacheHistory = new AddressCacheHistory(memoryCache, 2);
            addressCacheHistory.Add("1", memoryCache.GetCount());
            Assert.AreEqual(1, addressCacheHistory.History.Count);
            
            addressCacheHistory.Add("2", memoryCache.GetCount());
            Assert.AreEqual(2, addressCacheHistory.History.Count);

            addressCacheHistory.Add("3", memoryCache.GetCount());
            Assert.AreEqual(3, addressCacheHistory.History.Count);

            addressCacheHistory.Add("4", memoryCache.GetCount());
            Assert.AreEqual(4, addressCacheHistory.History.Count);

            addressCacheHistory.Add("5", memoryCache.GetCount());
            Assert.AreEqual(3, addressCacheHistory.History.Count);

            addressCacheHistory.Add("6", memoryCache.GetCount());
            Assert.AreEqual(4, addressCacheHistory.History.Count);
        }
    }
}