namespace SmsService.Api.Services.RateLimiter.Models
{
    public record RateLimitResult(bool IsAllowed, int RemainingQuota);
}