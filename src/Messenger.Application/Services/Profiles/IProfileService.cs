using Messenger.Application.DataTransferObjects.Auth.UserProfiles;
using Messenger.Application.DataTransferObjects.Auth;

namespace Messenger.Application.Services.Profiles
{
    public interface IProfileService
    {
        Task<UserProfile> GetUserProfileAsync();
        Task<UserProfile> GetUserProfileAsync(long userId);
        Task<UserProfile> UpdateUserProfileAsync(UserProfileModificationDto userProfileModificationDto);

        Task DeleteUserProfileAsync();
        Task<UserProfile> ConfirmDeleteProfileAsync(EmailConfirmationDto emailConfirmationDto);
    }
}
