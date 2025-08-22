using Moq;
using FluentAssertions;
using SmsService.Api.Services.RateLimiter;
using SmsService.Api.Services.RateLimiter.Models;
using System.Threading.Tasks;
using System;
using Xunit;

namespace SmsService.UnitTests.Services.RateLimiter;

public class RateLimiterServiceTests
{
    private readonly Mock<IRateLimiter> _limiterMock = new();
    private readonly RateLimiterService _service;

    public RateLimiterServiceTests()
    {
        _service = new RateLimiterService(_limiterMock.Object);
    }

    [Fact]
    public async Task CheckLimitAsync_Allowed_ReturnsAllowedResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _limiterMock.Setup(x => x.TryDecrementAsync(userId, 5))
            .ReturnsAsync(new RateLimitResult(true, 95));

        // Act
        var result = await _service.CheckLimitAsync(userId, 5);

        // Assert
        result.IsAllowed.Should().BeTrue();
        result.RemainingQuota.Should().Be(95);
    }

    [Fact]
    public async Task CheckLimitAsync_NotAllowed_ReturnsNotAllowedResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _limiterMock.Setup(x => x.TryDecrementAsync(userId, 10))
            .ReturnsAsync(new RateLimitResult(false, 5));

        // Act
        var result = await _service.CheckLimitAsync(userId, 10);

        // Assert
        result.IsAllowed.Should().BeFalse();
        result.RemainingQuota.Should().Be(5);
    }
}