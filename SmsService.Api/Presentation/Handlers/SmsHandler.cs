using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using SmsService.Api.Contracts.Requests;
using SmsService.Api.Services.RateLimiter;
using SmsService.Api.Services.RateLimiter.Models;

namespace SmsService.Api.Presentation.Handlers
{
    /// <summary>
    /// Обработчики эндпоинтов для работы с SMS
    /// </summary>
    public static class SmsHandler
    {
        /// <summary>
        /// Регистрирует эндпоинты для работы с SMS
        /// </summary>
        public static void MapSmsEndpoints(this WebApplication app)
        {
            app.MapPost("/api/sms/send", SendAsync);
        }

        /// <summary>
        /// Обрабатывает запрос на отправку SMS сообщений
        /// </summary>
        private static async Task<Results<Ok<SendSmsResponse>, ValidationProblem, ProblemHttpResult>> SendAsync(
            SendSmsRequest request,
            RateLimiterService rateLimiterService,
            IValidator<SendSmsRequest> validator,
            CancellationToken ct)
        {
            // Валидация запроса
            var validationResult = await validator.ValidateAsync(request, ct);
            if (!validationResult.IsValid)
            {
                return TypedResults.ValidationProblem(validationResult.ToDictionary());
            }

            // Проверка лимита
            var limitResult = await rateLimiterService.CheckLimitAsync(
                request.userId,
                request.phoneNumbers.Length);

            if (!limitResult.IsAllowed)
            {
                return TypedResults.Problem(
                    title: "Too Many Requests",
                    detail: "Daily SMS limit exceeded",
                    statusCode: StatusCodes.Status429TooManyRequests,
                    extensions: new Dictionary<string, object?>
                    {
                        { "remainingQuota", limitResult.RemainingQuota }
                    });
            }

            // Эмуляция отправки SMS
            await Task.Delay(500, ct);

            return TypedResults.Ok(new SendSmsResponse
            {
                SentCount = request.phoneNumbers.Length,
                RemainingQuota = limitResult.RemainingQuota
            });
        }
    }
}