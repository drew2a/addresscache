using System;

namespace AddressCacheProject
{
    public class CacheEntry
    {
        public Uri Data { get; }

        private readonly DateTime _expirationTime;
        public CacheEntry Next { get; set; }
        public CacheEntry Previous { get; set; }


        public CacheEntry(Uri uri, DateTime expirationTime, CacheEntry previous= null, CacheEntry next = null)
        {
            Data = uri;
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