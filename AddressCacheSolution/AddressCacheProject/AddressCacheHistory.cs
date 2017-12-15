using System.Collections.Generic;

namespace AddressCacheProject
{
    public class AddressCacheHistory
    {
        public LinkedList<string> History { get; } = new LinkedList<string>();

        public void Add(string key, long cacheCount)
        {
            History.AddFirst(key);

            var historyCount = History.Count;
            for (var i = cacheCount; i < historyCount; i++)
            {
                History.RemoveLast();
            }
        }
    }
}