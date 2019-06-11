using System;
using System.Threading.Tasks;

namespace KL.RateLimiter
{
    /// <summary>
    /// Queue designed for rate limiter
    /// </summary>
    public interface IRateLimiterQueue
    {
        /// <summary>
        /// Length
        /// </summary>
        /// <returns>length</returns>
        Task<long> LengthAsync();

        /// <summary>
        /// Push a timestamp to the queue
        /// </summary>
        /// <param name="timestamp">timestamp in unix milliseconds</param>
        /// <returns></returns>
        Task PushAsync(long timestamp);

        /// <summary>
        /// Get the oldest member in the queue
        /// </summary>
        /// <returns>oldest timestamp in the queue</returns>
        Task<long?> OldestAsync();

        /// <summary>
        /// Trim
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        Task TrimAsync(int length);
    }
}
