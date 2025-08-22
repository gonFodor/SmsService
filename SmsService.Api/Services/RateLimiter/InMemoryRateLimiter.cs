using System.Collections.Concurrent;
using SmsService.Api.Services.RateLimiter.Models;

namespace SmsService.Api.Services.RateLimiter
{
    /// <summary>
    /// In-memory реализаци€ rate limiting с использованием потокобезопасных коллекций
    /// </summary>
    public sealed class InMemoryRateLimiter : IRateLimiter
    {
        private const int DailyLimit = 100;
        private readonly ConcurrentDictionary<Guid, int> _counters = new();
        private readonly ConcurrentDictionary<Guid, SemaphoreSlim> _userLocks = new();

        /// <summary>
        /// ѕытаетс€ уменьшить счетчик лимита дл€ пользовател€
        /// </summary>
        public async Task<RateLimitResult> TryDecrementAsync(Guid userId, int count)
        {
            if (count <= 0)
                return new RateLimitResult(true, DailyLimit);

            var userLock = _userLocks.GetOrAdd(userId, _ => new SemaphoreSlim(1, 1));
            await userLock.WaitAsync();

            try
            {
                var current = _counters.GetOrAdd(userId, DailyLimit);

                if (current < count)
                    return new RateLimitResult(false, current);

                _counters[userId] = current - count;
                return new RateLimitResult(true, current - count);
            }
            finally
            {
                userLock.Release();
            }
        }

        /// <summary>
        /// —брасывает все счетчики лимитов
        /// </summary>
        public Task ResetAllAsync()
        {
            _counters.Clear();
            return Task.CompletedTask;
        }
    }
}