namespace SmsService.Api.Services.RateLimiter.Models
{
    /// <summary>
    /// Ответ на успешную отправку SMS сообщения
    /// </summary>
    public class SendSmsResponse
    {
        /// <summary>
        /// Количество отправленных SMS сообщений
        /// </summary>
        public int SentCount { get; set; }

        /// <summary>
        /// Оставшийся дневной лимит пользователя
        /// </summary>
        public int RemainingQuota { get; set; }
    }
}