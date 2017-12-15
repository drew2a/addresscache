using System;
using System.Security.Policy;
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
            var addressCache = new AddressCache(new TimeSpan());
            Assert.True(addressCache.Add(new Url("some.url")));
        }
    }
}