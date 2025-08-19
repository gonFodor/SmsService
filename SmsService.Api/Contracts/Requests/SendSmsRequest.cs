namespace SmsService.Api.Contracts.Requests
{
    public record SendSmsRequest(
    Guid userId,
    string message,
    string[] phoneNumbers);
}