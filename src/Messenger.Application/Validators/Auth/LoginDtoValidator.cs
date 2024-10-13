using FluentValidation;
using Messenger.Application.DataTransferObjects.Auth;

namespace Messenger.Application.Validators.Auth
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            // Email validation: required and must be a valid email format
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email maydoni bo'sh bo'lmasligi kerak.")
                .EmailAddress().WithMessage("Yaroqli email kiriting.");

            // Password validation: required and must be a valid password format
            RuleFor(dto => dto.Password)
                .NotEmpty().WithMessage("Parol bo'sh bo'lmasligi kerak.")
                .Length(8, 100).WithMessage("Parol kamida 8 ta belgidan iborat bo'lishi kerak.")
                .Matches(@"[A-Z]").WithMessage("Parolda kamida bitta katta harf bo'lishi kerak.")
                .Matches(@"[a-z]").WithMessage("Parolda kamida bitta kichik harf bo'lishi kerak.")
                .Matches(@"[0-9]").WithMessage("Parolda kamida bitta raqam bo'lishi kerak.");
        }
    }
}
