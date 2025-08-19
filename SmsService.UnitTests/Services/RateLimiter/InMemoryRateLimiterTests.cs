using FluentAssertions;
using SmsService.Api.Services.RateLimiter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmsService.UnitTests.Services.RateLimiter
{
    public class InMemoryRateLimiterTests
    {
        private readonly InMemoryRateLimiter _limiter = new();

        [Fact]
        public async Task TryDecrementAsync_FirstRequest_ReturnsFullQuota()
        {
            var result = await _limiter.TryDecrementAsync(Guid.NewGuid(), 10);

            result.IsAllowed.Should().BeTrue();
            result.RemainingQuota.Should().Be(90);
        }

        [Fact]
        public async Task TryDecrementAsync_WhenLimitExceeded_ReturnsFalse()
        {
            var userId = Guid.NewGuid();
            await _limiter.TryDecrementAsync(userId, 99);

            var result = await _limiter.TryDecrementAsync(userId, 2);

            result.IsAllowed.Should().BeFalse();
            result.RemainingQuota.Should().Be(1);
        }
    }
}