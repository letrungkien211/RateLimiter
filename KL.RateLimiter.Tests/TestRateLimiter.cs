using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace KL.RateLimiter.Tests
{
    public class TestRateLimiter
    {
        private ITestOutputHelper TestOutputHelper { get; }
        public TestRateLimiter(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }
        [Theory]
        [InlineData(0, 1000, 10, 5)]
        [InlineData(0, 2000, 20, 2)]
        [InlineData(0, 3000, 5, 3)]
        [InlineData(0, 5000, 40, 2)]

        [InlineData(1, 1000, 10, 5)]
        [InlineData(1, 2000, 20, 2)]
        [InlineData(1, 3000, 5, 3)]
        [InlineData(1, 5000, 40, 2)]

        public async Task Rate(int queueType, int window, int limit, int times)
        {
            var limiter = queueType == 1 ? new RateLimiter().UseDefaultQueue() : new RateLimiter().UseRedisQueue(StackExchange.Redis.ConnectionMultiplexer.Connect("localhost:6379,ssl=False,abortConnect=False").GetDatabase(), "UnitTest");

            var stopWatch = Stopwatch.StartNew();

            double passed = 0;
            double total = 0;

            while (stopWatch.ElapsedMilliseconds < window * times)
            {
                var (res, _) = await limiter.ReachLimitAsync(window, limit);
                passed += res ? 0 : 1;
                total++;
                await Task.Delay(1);
            }

            TestOutputHelper.WriteLine($"{passed}, {total}, Rate={passed / total}");

            Assert.Equal(limit * times, passed);
        }
    }
}
