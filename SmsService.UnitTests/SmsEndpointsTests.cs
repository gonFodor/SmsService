using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace SmsService.UnitTests;

public class SmsEndpointsTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public SmsEndpointsTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task SendSms_ValidRequest_ReturnsOk()
    {
        // Arrange
        var request = new
        {
            userId = Guid.NewGuid(),
            message = "Test message",
            phoneNumbers = new[] { "+1234567890" }
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/api/sms/send", content);

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.OK ||
                   response.StatusCode == HttpStatusCode.TooManyRequests);
    }

    [Fact]
    public async Task SendSms_InvalidPhoneNumber_ReturnsBadRequest()
    {
        // Arrange
        var request = new
        {
            userId = Guid.NewGuid(),
            message = "Test message",
            phoneNumbers = new[] { "invalid_number" } // Неправильный номер
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/api/sms/send", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SendSms_MessageTooLong_ReturnsBadRequest()
    {
        // Arrange
        var longMessage = new string('a', 501); // 501 символов
        var request = new
        {
            userId = Guid.NewGuid(),
            message = longMessage,
            phoneNumbers = new[] { "+1234567890" }
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/api/sms/send", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SendSms_EmptyPhoneNumbers_ReturnsBadRequest()
    {
        // Arrange
        var request = new
        {
            userId = Guid.NewGuid(),
            message = "Test message",
            phoneNumbers = Array.Empty<string>() // Пустой массив
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/api/sms/send", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}