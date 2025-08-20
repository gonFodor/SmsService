using SmsService.Api.Services.RateLimiter.Models;

namespace SmsService.Api.Services.RateLimiter
{
    public interface IRateLimiter
    {
        Task<RateLimitResult> TryDecrementAsync(Guid userId, int count);
        Task ResetAllAsync();
    }
}