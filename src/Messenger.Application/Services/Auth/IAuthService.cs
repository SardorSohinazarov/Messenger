using Messenger.Application.DataTransferObjects.Auth;
using Messenger.Application.DataTransferObjects.Auth.UserProfiles;

namespace Messenger.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<TokenDto> LoginAsync(LoginDto loginDto);
        //Task<TokenDto> LoginWithGoogleAccountAsync(GoogleLoginDto googleLoginDto);
        //Task<TokenDto> LoginWithFacebookAccountAsync(string returnUrl = "/");
        //Task<TokenDto> LoginWithTwitterAccountAsync(string returnUrl = "/");
        //Task<TokenDto> LoginWithMicrosoftAccountAsync(string returnUrl = "/");
        //Task<TokenDto> LoginWithTelegramAccountAsync(string returnUrl = "/");

        Task RegisterAsync(RegisterDto registerDto);
        Task<TokenDto> ConfirmEmailAsync(EmailConfirmationDto emailConfirmationDto);
        Task<TokenDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
        Task<UserProfile> GetUserProfileAsync();
        Task<UserProfile> GetUserProfileAsync(long userId);
        Task<UserProfile> UpdateUserProfileAsync(UserProfileModificationDto userProfileModificationDto);
        Task DeleteUserProfileAsync();
        Task<UserProfile> ConfirmDeleteProfileAsync(EmailConfirmationDto emailConfirmationDto);
    }
}
