namespace SmsService.Api.Config;

public static class SwaggerConfigurator
{
    public static void ConfigureSwagger(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SMS Service API v1");
            });
        }
    }
}