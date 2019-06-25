using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
namespace KL.RateLimiter
{
    /// <summary>
    /// Rate limter queue
    /// </summary>
    public class RateLimiterQueue : IRateLimiterQueue
    {
        private ConcurrentQueue<double> TimeStamps { get; } = new ConcurrentQueue<double>();

        public async Task<long> LengthAsync()
        {
            return TimeStamps.Count;
        }
        public async Task<double?> OldestAsync()
        {
            return TimeStamps.TryPeek(out var ret) ? ret : (double?)null;
        }

        public async Task PushAsync(double timestamp)
        {
            TimeStamps.Enqueue(timestamp);
        }

        public async Task TrimAsync(int length)
        {
            while (TimeStamps.Count > length)
            {
                TimeStamps.TryDequeue(out var _);
            }
        }
    }
}
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously