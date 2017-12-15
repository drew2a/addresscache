using System;
using System.Runtime.Caching;
using System.Threading;

namespace AddressCacheProject
{ /*
 * The AddressCache has a max age for the elements it's storing, an add method 
 * for adding elements, a remove method for removing, a peek method which 
 * returns the most recently added element, and a take method which removes 
 * and returns the most recently added element.
 */
    public class AddressCache
    {
        private readonly TimeSpan _maxAge;
        private readonly MemoryCache _cache = new MemoryCache("AddressCache");
        private readonly ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();


        public AddressCache(TimeSpan maxAge)
        {
            _maxAge = maxAge;
        }

        /**
         * Add() method must store unique elements only (existing elements must be ignored). 
         * This will return true if the element was successfully added. 
         * @param address
         * @return
         */
        public bool Add(Uri address)
        {
            if (address == null)
            {
                return false;
            }
            var key = address.AbsoluteUri;

            _cacheLock.EnterReadLock();
            try
            {
                if (_cache[key] != null)
                {
                    return false;
                }
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }

            _cacheLock.EnterWriteLock();
            try
            {
                _cache.Add(key, address, DateTime.Now.Add(_maxAge));
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }

            return true;
        }

        /**
         * Remove() method will return true if the address was successfully removed
         * @param address
         * @return
         */
        public bool Remove(Uri address)
        {
            if (address == null)
            {
                return false;
            }

            return true;
        }

        /**
         * The Peek() method will return the most recently added element, 
         * null if no element exists.
         * @return
         */
        public Uri Peek()
        {
            return null;
        }

        /**
         * Take() method retrieves and removes the most recently added element 
         * from the cache and waits if necessary until an element becomes available.
         * @return
         */
        public Uri Take()
        {
            return null;
        }
    }
}