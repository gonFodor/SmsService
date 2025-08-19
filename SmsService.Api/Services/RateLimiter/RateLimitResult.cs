namespace SmsService.Api.Services.RateLimiter
{
    public record RateLimitResult(bool IsAllowed, int RemainingQuota);
}