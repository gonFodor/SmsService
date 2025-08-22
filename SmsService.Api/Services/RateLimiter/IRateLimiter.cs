using SmsService.Api.Services.RateLimiter.Models;

namespace SmsService.Api.Services.RateLimiter
{
    /// <summary>
    /// Интерфейс для реализации механизма ограничения запросов (rate limiting)
    /// </summary>
    public interface IRateLimiter
    {
        /// <summary>
        /// Пытается уменьшить счетчик лимита для указанного пользователя
        /// </summary>
        /// <param name="userId">Уникальный идентификатор пользователя</param>
        /// <param name="count">Количество запрашиваемых единиц лимита (обычно SMS сообщений)</param>
        /// <returns>
        /// Результат операции, содержащий флаг разрешения и оставшийся лимит.
        /// Если операция разрешена - счетчик уменьшается на указанное количество.
        /// </returns>
        Task<RateLimitResult> TryDecrementAsync(Guid userId, int count);

        /// <summary>
        /// Сбрасывает все текущие счетчики лимитов для всех пользователей
        /// </summary>
        /// <remarks>
        /// Используется для:
        /// - Тестирования и отладки
        /// - Административных задач
        /// - Сброса лимитов в начале нового периода (например, ежедневно)
        /// </remarks>
        /// <returns>Задача, представляющая асинхронную операцию сброса</returns>
        Task ResetAllAsync();
    }
}