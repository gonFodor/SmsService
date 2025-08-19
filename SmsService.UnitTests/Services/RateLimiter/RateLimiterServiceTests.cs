using FluentAssertions;
using Moq;
using SmsService.Api.Services.RateLimiter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmsService.UnitTests.Services.RateLimiter
{
    public class RateLimiterServiceTests
    {
        private readonly Mock<IRateLimiter> _limiterMock = new();
        private readonly RateLimiterService _service;

        public RateLimiterServiceTests()
        {
            _service = new RateLimiterService(_limiterMock.Object);
        }

        [Fact]
        public async Task CheckLimitAsync_DelegatesToRateLimiter()
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
    }
}