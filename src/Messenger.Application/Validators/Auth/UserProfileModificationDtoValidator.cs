using FluentValidation;
using Messenger.Application.DataTransferObjects.Auth.UserProfiles;

namespace Messenger.Application.Validators.Auth
{
    public class UserProfileModificationDtoValidator : AbstractValidator<UserProfileModificationDto>
    {
        public UserProfileModificationDtoValidator()
        {
            // todo
        }
    }
}
