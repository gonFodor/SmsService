using Microsoft.Extensions.DependencyInjection;
using SmsService.Api.Config;
using FluentValidation;
using SmsService.Api.Contracts.Requests;
using SmsService.Api.Services.RateLimiter;
using Xunit;
using Microsoft.AspNetCore.Builder;

namespace SmsService.UnitTests;

public class ServiceConfiguratorTests
{
    [Fact]
    public void ConfigureServices_RegistersValidator()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();

        // Act
        ServiceConfigurator.ConfigureServices(builder);

        // Assert
        var serviceProvider = builder.Services.BuildServiceProvider();
        var validator = serviceProvider.GetService<IValidator<SendSmsRequest>>();
        Assert.NotNull(validator);
    }

    [Fact]
    public void ConfigureServices_RegistersRateLimiter()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();

        // Act
        ServiceConfigurator.ConfigureServices(builder);

        // Assert
        var serviceProvider = builder.Services.BuildServiceProvider();
        var rateLimiter = serviceProvider.GetService<IRateLimiter>();
        Assert.NotNull(rateLimiter);
    }

    [Fact]
    public void ConfigureServices_RegistersRateLimiterService()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();

        // Act
        ServiceConfigurator.ConfigureServices(builder);

        // Assert
        var serviceProvider = builder.Services.BuildServiceProvider();
        var rateLimiterService = serviceProvider.GetService<RateLimiterService>();
        Assert.NotNull(rateLimiterService);
    }

    [Fact]
    public void ConfigureServices_RegistersCors()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();

        // Act
        ServiceConfigurator.ConfigureServices(builder);

        // Assert
        Assert.Contains(builder.Services, s => s.ServiceType.Name.Contains("Cors"));
    }
}