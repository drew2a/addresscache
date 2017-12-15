using System;
using System.Runtime.Caching;
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
        private readonly MemoryCache _cache;
        public AddressCacheHistory _cacheHistory;
        private readonly ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();

        public AddressCache(TimeSpan maxAge, int historyStartRecalculateDifference = 100)
        {
            _maxAge = maxAge;
            _cache = new MemoryCache("AddressCache");
            _cacheHistory = new AddressCacheHistory(_cache, historyStartRecalculateDifference);
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

            _cacheLock.EnterUpgradeableReadLock();
            try
            {
                if (_cache[key] != null)
                {
                    return false;
                }
                _cacheLock.EnterWriteLock();
                try
                {
                    _cache.Add(key, address, DateTime.Now.Add(_maxAge));
                    _cacheHistory.Add(key, _cache.GetCount());
                }
                finally
                {
                    _cacheLock.ExitWriteLock();
                }
            }
            finally
            {
                _cacheLock.ExitUpgradeableReadLock();
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

            _cacheLock.EnterUpgradeableReadLock();
            try
            {
                if (_cache[key] == null)
                {
                    return false;
                }
                _cacheLock.EnterWriteLock();
                try
                {
                    _cache.Remove(key);
                }
                finally
                {
                    _cacheLock.ExitWriteLock();
                }
            }
            finally
            {
                _cacheLock.ExitUpgradeableReadLock();
            }

            return true;
        }

        /// <summary>
        /// The Peek() method will return the most recently added element,
        /// null if no element exists. 
        /// </summary>
        /// <returns></returns>
        public Uri Peek()
        {
            _cacheLock.EnterReadLock();
            try
            {
                if (_cacheHistory.History.Count == 0)
                {
                    return null;
                }

                string resentUrl;
                if (_cacheHistory.History.TryPeek(out resentUrl))
                {
                    return _cache[resentUrl] as Uri;
                }

                throw new TryPeekException();
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Take() method retrieves and removes the most recently added element
        /// from the cache and waits if necessary until an element becomes available.
        /// </summary>
        /// <returns></returns>
        public Uri Take()
        {
            return null;
        }

        /// <summary>
        /// Count() method retrieves count of cashed elements
        /// </summary>
        /// <returns></returns>
        public long Count()
        {
            _cacheLock.EnterReadLock();
            try
            {
                return _cache.GetCount();
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }
        }
    }
}