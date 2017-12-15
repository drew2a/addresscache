using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.Caching;

namespace AddressCacheProject
{
    public class AddressCacheHistory
    {
        private readonly MemoryCache _memoryCache;
        private readonly int _startCleanDifference;

        public ConcurrentStack<string> History { get; set; } = new ConcurrentStack<string>();

        public AddressCacheHistory(MemoryCache memoryCache, int startCleanDifference)
        {
            _memoryCache = memoryCache;
            _startCleanDifference = startCleanDifference > 0 ? startCleanDifference : 0;
        }

        public void Add(string key, long casheCount)
        {
            History.Push(key);
            Clean(casheCount);
        }

        private void Clean(long cacheCount)
        {
            var needRecalculate = History.Count >= cacheCount + _startCleanDifference;
            if (!needRecalculate)
            {
                return;
            }
            var updatedHistory = new ConcurrentStack<string>();
            foreach (var item in History.Reverse())
            {
                if (_memoryCache[item] != null)
                {
                    updatedHistory.Push(item);
                }
            }

            History = updatedHistory;
        }
    }
}