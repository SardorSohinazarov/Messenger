using FluentValidation;
using Messenger.Application.DataTransferObjects.Auth;

namespace Messenger.Application.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            // FirstName validatsiyasi
            RuleFor(dto => dto.FirstName)
                .NotEmpty().WithMessage("Ism bo'sh bo'lmasligi kerak.")
                .Length(2, 50).WithMessage("Ism uzunligi 2 dan 50 gacha bo'lishi kerak.");

            // LastName validatsiyasi (ixtiyoriy)
            RuleFor(dto => dto.LastName)
                .Length(0, 50).WithMessage("Familiya uzunligi 0 dan 50 gacha bo'lishi kerak.");

            // PhoneNumber validatsiyasi
            RuleFor(dto => dto.PhoneNumber)
                .NotEmpty().WithMessage("Telefon raqami bo'sh bo'lmasligi kerak.")
                .Matches(@"^\d{9}$").WithMessage("Telefon raqami 9 ta raqamdan iborat bo'lishi kerak.");

            // Email validatsiyasi (ixtiyoriy)
            RuleFor(dto => dto.Email)
                .EmailAddress().WithMessage("Email formati noto'g'ri.")
                .When(dto => !string.IsNullOrEmpty(dto.Email)); // Email ixtiyoriy bo'lsa

            // Password validatsiyasi
            RuleFor(dto => dto.Password)
                .NotEmpty().WithMessage("Parol bo'sh bo'lmasligi kerak.")
                .Length(8, 100).WithMessage("Parol kamida 8 ta belgidan iborat bo'lishi kerak.")
                .Matches(@"[A-Z]").WithMessage("Parolda kamida bitta katta harf bo'lishi kerak.")
                .Matches(@"[a-z]").WithMessage("Parolda kamida bitta kichik harf bo'lishi kerak.")
                .Matches(@"[0-9]").WithMessage("Parolda kamida bitta raqam bo'lishi kerak.");
        }
    }
}
