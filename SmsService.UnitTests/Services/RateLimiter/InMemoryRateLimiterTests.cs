using FluentAssertions;
using SmsService.Api.Services.RateLimiter;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SmsService.UnitTests.Services.RateLimiter;

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
    public async Task TryDecrementAsync_ExceedLimit_ReturnsFalse()
    {
        var userId = Guid.NewGuid();
        await _limiter.TryDecrementAsync(userId, 99);

        var result = await _limiter.TryDecrementAsync(userId, 2);

        result.IsAllowed.Should().BeFalse();
        result.RemainingQuota.Should().Be(1);
    }

    [Fact]
    public async Task TryDecrementAsync_ZeroCount_ReturnsTrue()
    {
        var result = await _limiter.TryDecrementAsync(Guid.NewGuid(), 0);
        result.IsAllowed.Should().BeTrue();
        result.RemainingQuota.Should().Be(100);
    }

    [Fact]
    public async Task TryDecrementAsync_NegativeCount_ReturnsTrue()
    {
        var result = await _limiter.TryDecrementAsync(Guid.NewGuid(), -5);
        result.IsAllowed.Should().BeTrue();
        result.RemainingQuota.Should().Be(100);
    }

    [Fact]
    public async Task TryDecrementAsync_DifferentUsers_IsolatedCounters()
    {
        var user1 = Guid.NewGuid();
        var user2 = Guid.NewGuid();

        await _limiter.TryDecrementAsync(user1, 50);
        var result2 = await _limiter.TryDecrementAsync(user2, 100);

        result2.IsAllowed.Should().BeTrue();
        result2.RemainingQuota.Should().Be(0);
    }
}