using FluentValidation;
using Messenger.Application.DataTransferObjects.Auth;

namespace Messenger.Application.Validators
{
    public class RefreshTokenDtoValidator : AbstractValidator<RefreshTokenDto>
    {
        public RefreshTokenDtoValidator()
        {
            // Access token validation: required and must not be empty
            RuleFor(x => x.AccessToken)
                .NotEmpty().WithMessage("Access token bo'sh bo'lmasligi kerak.");

            // Refresh token validation: required and must not be empty
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token bo'sh bo'lmasligi kerak.");
        }
    }
}
