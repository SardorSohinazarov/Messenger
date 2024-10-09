using FluentValidation;
using Messenger.Application.DataTransferObjects.Auth;

namespace Messenger.Application.Validators
{
    public class EmailConfirmationDtoValidator : AbstractValidator<EmailConfirmationDto>
    {
        public EmailConfirmationDtoValidator()
        {
            // Email validation: required and must be a valid email format
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email maydoni bo'sh bo'lmasligi kerak.")
                .EmailAddress().WithMessage("Yaroqli email kiriting.");

            // Confirmation code validation: required, must be exactly 4 digits
            RuleFor(x => x.ConfirmationCode)
                .NotEmpty().WithMessage("Tasdiqlash kodi bo'sh bo'lmasligi kerak.")
                .Matches(@"^\d{4}$").WithMessage("Tasdiqlash kodi 4 ta raqamdan iborat bo'lishi kerak."); // Only 4 digits allowed
        }
    }
}
