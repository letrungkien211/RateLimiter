using System;
using System.Collections.Generic;
using System.Text;

namespace KL.RateLimiter
{
    public static class Extensions
    {
        public static RateLimiter UseDefaultQueue(this RateLimiter rateLimiter)
        {
            rateLimiter.UseQueue(new RateLimiterQueue());
            return rateLimiter;
        }
    }
}
