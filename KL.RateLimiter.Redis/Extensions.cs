using StackExchange.Redis;

namespace KL.RateLimiter.Redis
{
    public static class Extensions
    {
        public static RateLimiter UseRedisQueue(this RateLimiter rateLimiter, IDatabase database, string queueName)
        {
            rateLimiter.UseQueue(new RedisRateLimiterQueue(database, queueName));
            return rateLimiter;
        }
    }
}
