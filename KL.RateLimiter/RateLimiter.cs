using System;
using System.Threading.Tasks;

namespace KL.RateLimiter
{
    public class RateLimiter
    {
        private IRateLimiterQueue Queue { get; set; }
        public RateLimiter() { }
        public void UseQueue(IRateLimiterQueue rateLimiterQueue)
        {
            Queue = rateLimiterQueue;
        }

        public async Task<(bool, long)> ReachLimitAsync(int windowMillis, int limit)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var ret = false;
            var length = await Queue.LengthAsync();
            if (length < limit)
            {
                await Queue.PushAsync(now);
            }
            else
            {
                var oldest = await Queue.OldestAsync();
                if (oldest != null && now - oldest < windowMillis)
                {
                    ret = true;
                }
                else
                {
                    await Queue.PushAsync(now);
                }
            }

            await Queue.TrimAsync(limit);

            return (ret, length);
        }
    }
}
