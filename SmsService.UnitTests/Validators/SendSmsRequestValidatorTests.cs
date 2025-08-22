using FluentValidation.TestHelper;
using SmsService.Api.Contracts.Requests;
using SmsService.Api.Validators;
using System;
using Xunit;

namespace SmsService.UnitTests.Validators;

public class SendSmsRequestValidatorTests
{
    private readonly SendSmsRequestValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Validate_EmptyMessage_ShouldFail(string message)
    {
        var request = new SendSmsRequest(Guid.NewGuid(), message, new[] { "+1234567890" });
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.message);
    }

    [Fact]
    public void Validate_MessageTooLong_ShouldFail()
    {
        var longMessage = new string('a', 501);
        var request = new SendSmsRequest(Guid.NewGuid(), longMessage, new[] { "+1234567890" });
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.message);
    }

    [Fact]
    public void Validate_EmptyPhoneNumbers_ShouldFail()
    {
        var request = new SendSmsRequest(Guid.NewGuid(), "test", Array.Empty<string>());
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.phoneNumbers);
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("1234567890")]
    [InlineData("+abc")]
    public void Validate_InvalidPhoneNumber_ShouldFail(string phoneNumber)
    {
        var request = new SendSmsRequest(Guid.NewGuid(), "test", new[] { phoneNumber });
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.phoneNumbers);
    }

    [Theory]
    [InlineData("+1234567890")]
    [InlineData("+380441234567")]
    [InlineData("+123456789012345")] // 15 digits
    public void Validate_ValidPhoneNumber_ShouldPass(string phoneNumber)
    {
        var request = new SendSmsRequest(Guid.NewGuid(), "test", new[] { phoneNumber });
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.phoneNumbers);
    }

    [Fact]
    public void Validate_ValidRequest_ShouldPass()
    {
        var request = new SendSmsRequest(
            Guid.NewGuid(),
            "Valid message",
            new[] { "+1234567890" });

        var result = _validator.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }
}