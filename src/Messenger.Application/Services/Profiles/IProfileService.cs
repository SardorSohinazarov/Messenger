using Messenger.Application.DataTransferObjects.Auth.UserProfiles;
using Messenger.Application.DataTransferObjects.Auth;
using Messenger.Application.Common.Results;

namespace Messenger.Application.Services.Profiles
{
    public interface IProfileService
    {
        Task<Result<UserProfile>> GetUserProfileAsync();
        Task<Result<UserProfile>> GetUserProfileAsync(long userId);
        Task<Result<UserProfile>> UpdateUserProfileAsync(UserProfileModificationDto userProfileModificationDto);

        Task<Result> DeleteUserProfileAsync();
        Task<Result<UserProfile>> ConfirmDeleteProfileAsync(EmailConfirmationDto emailConfirmationDto);
    }
}
