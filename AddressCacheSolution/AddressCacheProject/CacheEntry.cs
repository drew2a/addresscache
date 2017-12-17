using System;
using System.Net;

namespace AddressCacheProject
{
    public class CacheEntry
    {
        public IPAddress Address { get; }
        public CacheEntry Next { get; set; }
        public CacheEntry Previous { get; set; }

        private readonly DateTime _expirationTime;

        public CacheEntry(IPAddress address, DateTime expirationTime, CacheEntry previous = null,
            CacheEntry next = null)
        {
            Address = address;
            Next = next;
            Previous = previous;

            _expirationTime = expirationTime;
        }

        public bool IsExpired()
        {
            return DateTime.Now > _expirationTime;
        }
    }
}