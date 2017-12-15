using System.Collections.Generic;

namespace AddressCacheProject
{
    public class AddressCacheHistory
    {
        private readonly LinkedList<string> _history = new LinkedList<string>();

        public void Add(string key, long cacheCount)
        {
            _history.AddFirst(key);

            var historyCount = _history.Count;
            for (var i = cacheCount; i < historyCount; i++)
            {
                _history.RemoveLast();
            }
        }

        public string Recent()
        {
            return _history.First?.Value;
        }

        public long Count()
        {
            return _history.Count;
        }

        public void Remove(string key)
        {
            _history.Remove(key);
        }
    }
}