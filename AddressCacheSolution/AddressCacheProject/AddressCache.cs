using System;
using System.Security.Policy;

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

        public bool Add(Url address)
        {
            return true;
        }

        /**
         * Remove() method will return true if the address was successfully removed
         * @param address
         * @return
         */
        public bool Remove(Url address)
        {
            return false;
        }

        /**
         * The Peek() method will return the most recently added element, 
         * null if no element exists.
         * @return
         */
        public Url Peek()
        {
            return null;
        }

        /**
         * Take() method retrieves and removes the most recently added element 
         * from the cache and waits if necessary until an element becomes available.
         * @return
         */
        public Url Take()
        {
            return null;
        }
    }
}