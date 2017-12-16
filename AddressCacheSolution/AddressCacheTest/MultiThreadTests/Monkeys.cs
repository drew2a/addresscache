using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AddressCacheProject;

namespace AddressCacheTest.MultiThreadTests
{
    public class Monkeys
    {
        private readonly int _count;
        private readonly int _actionCount;
        private readonly int _actionDelayInMs;
        private readonly int _taskStartDelay;

        private readonly AddressCache _addressCache;


        public Monkeys(TimeSpan maxCacheAge, int count = 100, int actionCount = 100, int actionDelayInMs = 100,
            int taskStartDelay = 100)
        {
            _count = count;
            _actionCount = actionCount;
            _actionDelayInMs = actionDelayInMs;
            _taskStartDelay = taskStartDelay;

            _addressCache = new AddressCache(maxCacheAge);
        }

        public void ReleaseTheMonkeys()
        {
            var tasks = new Task[_count];
            for (var i = 0; i < _count; i++)
            {
                tasks[i] = Task.Factory.StartNew(SingleMonkey);
                Thread.Sleep(_taskStartDelay);
            }

            Task.WaitAll(tasks);
        }

        public void SingleMonkey()
        {
            var random = new Random();
            for (var i = 0; i < _actionCount; i++)
            {
                var address = GenerateAddress(random);

                switch (random.Next(1, 4))
                {
                    case 1:
                        _addressCache.Add(address);
                        break;
                    case 2:
                        _addressCache.Remove(address);
                        break;
                    case 3:
                        _addressCache.Peek();
                        break;
                    default:
                        Console.WriteLine("In default");
                        break;
                }

                Thread.Sleep(_actionDelayInMs);
            }
        }

        public IPAddress GenerateAddress(Random random)
        {
            return IPAddress.Parse(String.Format("{0}.{1}.{2}.{3}",
                random.Next(0, 256),
                random.Next(0, 256),
                random.Next(0, 256),
                random.Next(0, 256)));
        }
    }
}