using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Linq;

namespace SmsService.UnitTests.Integration;

public class ConcurrentRequestsTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ConcurrentRequestsTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task MultipleConcurrentRequests_RespectRateLimit()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var tasks = new List<Task<HttpResponseMessage>>();
        var request = new
        {
            userId,
            message = "Test message",
            phoneNumbers = new[] { "+1234567890" }
        };

        // Act - отправляем 105 запросов (больше лимита)
        for (int i = 0; i < 105; i++)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            tasks.Add(_client.PostAsync("/api/sms/send", content));
        }

        var responses = await Task.WhenAll(tasks);

        // Assert - считаем успешные и неуспешные ответы
        var successCount = responses.Count(r => r.StatusCode == HttpStatusCode.OK);
        var tooManyRequestsCount = responses.Count(r => r.StatusCode == HttpStatusCode.TooManyRequests);

        Assert.True(successCount <= 100); // Не больше лимита
        Assert.True(tooManyRequestsCount >= 5); // Как минимум 5 отклоненных
    }
}