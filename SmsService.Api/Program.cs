using SmsService.Api.Config;
using SmsService.Api.Presentation;

var builder = WebApplication.CreateBuilder(args);

// Конфигурация сервисов
ServiceConfigurator.ConfigureServices(builder);

var app = builder.Build();

// Middleware
app.UseCors();
SwaggerConfigurator.ConfigureSwagger(app);

// Endpoints
app.MapSmsEndpoints();

app.Run();

public partial class Program { }