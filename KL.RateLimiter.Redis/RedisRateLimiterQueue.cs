﻿using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace KL.RateLimiter.Redis
{
    internal class RedisRateLimiterQueue : IRateLimiterQueue
    {
        private IDatabase Database { get; }
        private string QueueName { get; }
        public RedisRateLimiterQueue(IDatabase database, string queueName)
        {
            Database = database;
            QueueName = queueName;
        }

        public Task<long> LengthAsync()
        {
            return Database.ListLengthAsync(QueueName);
        }

        public async Task<long?> OldestAsync()
        {
            var res = await Database.ListGetByIndexAsync(QueueName, -1);
            return res.HasValue ? (long)res : (long?)null;
        }

        public Task PushAsync(long timestamp)
        {
            return Database.ListLeftPushAsync(QueueName, timestamp);
        }

        public Task TrimAsync(int length)
        {
            return Database.ListTrimAsync(QueueName, 0, length - 1);
        }
    }
}