using FluentValidation.TestHelper;
using SmsService.Api.Contracts.Requests;
using SmsService.Api.Validators;
using Xunit;

namespace SmsService.UnitTests.Validators
{
    public class SendSmsRequestValidatorTests
    {
        private readonly SendSmsRequestValidator _validator = new();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Message_Empty_ShouldFail(string message)
        {
            var model = new SendSmsRequest(
                userId: Guid.NewGuid(),
                message: message,
                phoneNumbers: new[] { "+1234567890" }
            );

            _validator.TestValidate(model)
                .ShouldHaveValidationErrorFor(x => x.message);
        }

        [Fact]
        public void PhoneNumbers_EmptyArray_ShouldFail()
        {
            var model = new SendSmsRequest(
                userId: Guid.NewGuid(),
                message: "Valid",
                phoneNumbers: Array.Empty<string>()
            );

            _validator.TestValidate(model)
                .ShouldHaveValidationErrorFor(x => x.phoneNumbers);
        }
    }
}