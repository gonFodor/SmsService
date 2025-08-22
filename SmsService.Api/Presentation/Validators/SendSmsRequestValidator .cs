using FluentValidation;
using SmsService.Api.Contracts.Requests;

namespace SmsService.Api.Validators
{
    public class SendSmsRequestValidator : AbstractValidator<SendSmsRequest>
    {
        public SendSmsRequestValidator()
        {
            RuleFor(x => x.userId)
                .NotEmpty().WithMessage("User ID must not be empty.");

            RuleFor(x => x.message)
                .NotEmpty().WithMessage("Message must not be empty.")
                .MaximumLength(500).WithMessage("Message length must be 500 characters or fewer.");

            RuleFor(x => x.phoneNumbers)
                .NotEmpty().WithMessage("Phone numbers must not be empty.")
                .Must(phones => phones.All(p => !string.IsNullOrWhiteSpace(p)))
                .WithMessage("All phone numbers must be valid.");

            RuleForEach(x => x.phoneNumbers)
                .Matches(@"^\+[1-9]\d{1,14}$")
                .WithMessage("Phone number must be in E.164 format.");
        }
    }
}