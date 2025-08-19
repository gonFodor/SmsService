using FluentValidation;
using Microsoft.OpenApi.Models;
using SmsService.Api.Contracts.Requests;
using SmsService.Api.Services.RateLimiter;
using SmsService.Api.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SMS Service API",
        Version = "v1",
        Description = "API for sending SMS with rate limiting"
    });
});
builder.Services.AddScoped<IValidator<SendSmsRequest>, SendSmsRequestValidator>();
builder.Services.AddSingleton<IRateLimiter, InMemoryRateLimiter>();
builder.Services.AddScoped<RateLimiterService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors();

// Включение middleware Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SMS Service API v1");
    });
}

app.MapPost("/api/sms/send", async (
    SendSmsRequest request,
    RateLimiterService rateLimiterService,
    IValidator<SendSmsRequest> validator,
    CancellationToken ct) =>
{
    // Валидация запроса
    var validationResult = await validator.ValidateAsync(request, ct);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    // Проверка лимита
    var limitResult = await rateLimiterService.CheckLimitAsync(
        request.userId,
        request.phoneNumbers.Length);

    if (!limitResult.IsAllowed)
    {
        return Results.Problem(
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

    return Results.Ok(new
    {
        sentCount = request.phoneNumbers.Length,
        remainingQuota = limitResult.RemainingQuota
    });
});

app.Run();

public partial class Program
{
    // Пустой класс, нужен только для ссылки в тестах
    protected Program() { }
}