using FluentValidation;
using Microsoft.OpenApi.Models;
using SmsService.Api.Contracts.Requests;
using SmsService.Api.Services.RateLimiter;
using SmsService.Api.Validators;

namespace SmsService.Api.Config;

public static class ServiceConfigurator
{
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
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
    }
}