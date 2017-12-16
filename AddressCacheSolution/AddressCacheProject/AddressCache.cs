using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AddressCacheProject
{
    /// <summary>
    /// The AddressCache has a max age for the elements it's storing, an add method 
    /// for adding elements, a remove method for removing, a peek method which
    /// returns the most recently added element, and a take method which removes
    /// and returns the most recently added element.
    /// </summary>
    public class AddressCache
    {
        private readonly TimeSpan _maxAge;

        private readonly Dictionary<string, CacheEntry> _cache = new Dictionary<string, CacheEntry>();
        private readonly object lockObject = new object();

        private CacheEntry _last = null;

        public AddressCache(TimeSpan maxAge)
        {
            _maxAge = maxAge;
        }

        /// <summary>
        /// Add() method must store unique elements only (existing elements must be ignored). 
        /// This will return true if the element was successfully added.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public bool Add(Uri address)
        {
            if (address == null)
            {
                return false;
            }

            var key = address.AbsoluteUri;

            lock (lockObject)
            {
                CacheEntry cacheEntry;
                var isEntryExists = _cache.TryGetValue(key, out cacheEntry);

                if (isEntryExists && !cacheEntry.IsExpired())
                {
                    return false;
                }

                cacheEntry = new CacheEntry(
                    uri: address,
                    expirationTime: DateTime.Now.Add(_maxAge),
                    next: _last);

                if (_last != null)
                {
                    _last.Previous = cacheEntry;
                }

                _cache[key] = cacheEntry;
                _last = cacheEntry;

                Monitor.PulseAll(lockObject);
            }

            return true;
        }

        /// <summary>
        /// Remove() method will return true if the address was successfully removed
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public bool Remove(Uri address)
        {
            if (address == null)
            {
                return false;
            }

            var key = address.AbsoluteUri;

            lock (lockObject)
            {
                return RemoveEntry(key);
            }
        }

        /// <summary>
        /// The Peek() method will return the most recently added element,
        /// null if no element exists. 
        /// </summary>
        /// <returns></returns>
        public Uri Peek()
        {
            lock (lockObject)
            {
                if (_last == null || _last.IsExpired())
                {
                    return null;
                }

                return _last.Data;
            }
        }

        /// <summary>
        /// Take() method retrieves and removes the most recently added element
        /// from the cache and waits if necessary until an element becomes available.
        /// </summary>
        /// <returns></returns>
        public Uri Take()
        {
            lock (lockObject)
            {
                RemoveExpired();

                while (_last == null)
                {
                    Monitor.Wait(lockObject);
                }

                var result = _last.Data;

                RemoveEntry(_last.Data.AbsoluteUri);
                return result;
            }
        }

        /// <summary>
        /// Count() method retrieves count of cashed elements
        /// </summary>
        /// <returns></returns>
        public long Count()
        {
            lock (lockObject)
            {
                RemoveExpired();
                return _cache.Count;
            }
        }

        private bool RemoveEntry(string key)
        {
            CacheEntry removingEntry;
            var isEntryExists = _cache.TryGetValue(key, out removingEntry);
            if (!isEntryExists)
            {
                return false;
            }

            if (_last == removingEntry)
            {
                _last = removingEntry.Next;
            }

            if (removingEntry.Previous != null)
            {
                removingEntry.Previous.Next = removingEntry.Next;
            }

            if (removingEntry.Next != null)
            {
                removingEntry.Next.Previous = removingEntry.Previous;
            }

            removingEntry.Next = removingEntry.Previous = null;

            _cache.Remove(key);
            return !removingEntry.IsExpired();
        }

        private void RemoveExpired()
        {
            var expired = _cache.Keys
                .Where(k => _cache[k].IsExpired())
                .ToArray();

            foreach (var key in expired)
            {
                RemoveEntry(key);
            }
        }
    }
}