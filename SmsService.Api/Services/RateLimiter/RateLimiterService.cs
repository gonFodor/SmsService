using System.Threading.RateLimiting;

namespace SmsService.Api.Services.RateLimiter
{
    public class RateLimiterService
    {
        private readonly IRateLimiter _rateLimiter;

        public RateLimiterService(IRateLimiter rateLimiter)
        {
            _rateLimiter = rateLimiter;
        }

        public async Task<RateLimitResult> CheckLimitAsync(Guid userId, int requestedCount)
        {
            return await _rateLimiter.TryDecrementAsync(userId, requestedCount);
        }
    }
}