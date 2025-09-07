namespace SmsService.Api.Services.RateLimiter.Models
{
    /// <summary>
    /// Результат проверки лимита отправки SMS
    /// </summary>
    public record RateLimitResult(bool IsAllowed, int RemainingQuota);
}