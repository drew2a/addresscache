using System;
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
                var uri = GenerateUri(random);

                switch (random.Next(1, 4))
                {
                    case 1:
                        _addressCache.Add(uri);
//                        Console.WriteLine("Add({0}): {1}", uri, _addressCache.Add(uri));
                        break;
                    case 2:
                        _addressCache.Remove(uri);
//                        Console.WriteLine("Remove({0}): {1}", uri, _addressCache.Remove(uri));
                        break;
                    case 3:
                        _addressCache.Peek();
//                        Console.WriteLine("Peek(): {0}", _addressCache.Peek());
                        break;
                    default:
                        Console.WriteLine("In default");
                        break;
                }

                Thread.Sleep(_actionDelayInMs);
            }
        }

        public Uri GenerateUri(Random random)
        {
            const string alpabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new Uri(String.Format("http://{0}.{1}",
                alpabet[random.Next(0, alpabet.Length)],
                alpabet[random.Next(0, alpabet.Length)]));
        }
    }
}