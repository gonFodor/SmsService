namespace SmsService.Api.Services.RateLimiter.Models
{
    public class SendSmsResponse
    {
        public int SentCount { get; set; }
        public int RemainingQuota { get; set; }
    }
}
