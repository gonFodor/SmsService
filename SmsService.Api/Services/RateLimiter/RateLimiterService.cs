using System.Threading.RateLimiting;
using SmsService.Api.Services.RateLimiter.Models;

namespace SmsService.Api.Services.RateLimiter
{
    /// <summary>
    /// Сервис для проверки и управления лимитами отправки SMS сообщений
    /// </summary>
    public class RateLimiterService
    {
        private readonly IRateLimiter _rateLimiter;

        /// <summary>
        /// Инициализирует новый экземпляр сервиса управления лимитами
        /// </summary>
        /// <param name="rateLimiter">Реализация интерфейса ограничения запросов</param>
        public RateLimiterService(IRateLimiter rateLimiter)
        {
            _rateLimiter = rateLimiter;
        }

        /// <summary>
        /// Проверяет возможность отправки указанного количества SMS сообщений для пользователя
        /// </summary>
        /// <param name="userId">Уникальный идентификатор пользователя</param>
        /// <param name="requestedCount">Запрашиваемое количество SMS сообщений для отправки</param>
        /// <returns>
        /// Результат проверки лимита, содержащий информацию о разрешении операции
        /// и оставшемся количестве доступных сообщений
        /// </returns>
        /// <exception cref="ArgumentException">Если requestedCount отрицательное число</exception>
        /// <example>
        /// Пример использования:
        /// <code>
        /// var result = await rateLimiterService.CheckLimitAsync(userId, phoneNumbers.Length);
        /// if (!result.IsAllowed) return Results.TooManyRequests();
        /// </code>
        /// </example>
        public async Task<RateLimitResult> CheckLimitAsync(Guid userId, int requestedCount)
        {
            if (requestedCount < 0)
                throw new ArgumentException("Requested count cannot be negative", nameof(requestedCount));

            return await _rateLimiter.TryDecrementAsync(userId, requestedCount);
        }
    }
}