using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.Caching;

namespace AddressCacheProject
{
    public class AddressCacheHistory
    {
        private readonly MemoryCache _memoryCache;
        private readonly int _startRecalculateDifference;

        public ConcurrentStack<string> History { get; set; } = new ConcurrentStack<string>();

        public AddressCacheHistory(MemoryCache memoryCache, int startRecalculateDifference)
        {
            _memoryCache = memoryCache;
            _startRecalculateDifference = startRecalculateDifference;
        }

        public void Add(string key, long casheCount)
        {
            History.Push(key);
            Recalculate(casheCount);
        }

        private void Recalculate(long cacheCount)
        {
            var needRecalculate =   History.Count >= cacheCount + _startRecalculateDifference;
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