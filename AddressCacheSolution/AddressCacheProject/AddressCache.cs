using System;
using System.Collections.Generic;
using System.Net;
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
        private readonly object _lockObject = new object();
        private CacheEntry _mostRecent;
        private CacheEntry _mostOld;

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
        public bool Add(IPAddress address)
        {
            if (address == null)
            {
                return false;
            }

            var key = address.ToString();

            lock (_lockObject)
            {
                CacheEntry cacheEntry;
                var isEntryExists = _cache.TryGetValue(key, out cacheEntry);
                if (isEntryExists)
                {
                    if (!cacheEntry.IsExpired())
                    {
                        return false;
                    }

                    RemoveEntry(key);
                }

                cacheEntry = new CacheEntry(
                    address: address,
                    expirationTime: DateTime.Now.Add(_maxAge),
                    next: _mostRecent);

                if (_mostRecent != null)
                {
                    _mostRecent.Previous = cacheEntry;
                }

                if (_mostOld == null)
                {
                    _mostOld = cacheEntry;
                }

                _cache[key] = cacheEntry;
                _mostRecent = cacheEntry;

                Monitor.PulseAll(_lockObject);
            }

            return true;
        }

        /// <summary>
        /// Remove() method will return true if the address was successfully removed
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public bool Remove(IPAddress address)
        {
            if (address == null)
            {
                return false;
            }

            var key = address.ToString();

            lock (_lockObject)
            {
                return RemoveEntry(key);
            }
        }

        /// <summary>
        /// The Peek() method will return the most recently added element,
        /// null if no element exists. 
        /// </summary>
        /// <returns></returns>
        public IPAddress Peek()
        {
            lock (_lockObject)
            {
                if (_mostRecent == null || _mostRecent.IsExpired())
                {
                    return null;
                }

                return _mostRecent.Address;
            }
        }

        /// <summary>
        /// Take() method retrieves and removes the most recently added element
        /// from the cache and waits if necessary until an element becomes available.
        /// </summary>
        /// <returns></returns>
        public IPAddress Take()
        {
            lock (_lockObject)
            {
                RemoveExpired();

                while (_mostRecent == null)
                {
                    Monitor.Wait(_lockObject);
                }

                var result = _mostRecent.Address;

                RemoveEntry(_mostRecent.Address.ToString());
                return result;
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

            if (_mostRecent == removingEntry)
            {
                _mostRecent = _mostRecent.Next;
            }

            if (_mostOld == removingEntry)
            {
                _mostOld = _mostOld.Previous;
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
            var pointer = _mostOld;
            while (pointer != null && pointer.IsExpired())
            {
                RemoveEntry(pointer.Address.ToString());
                pointer = _mostOld;
            }
        }
    }
}