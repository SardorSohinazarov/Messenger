using Messenger.Application.Models.DataTransferObjects.Auth;
using Messenger.Application.Models.Results;

namespace Messenger.Application.Services.Auth
{
    public interface IAuthService
    {
        #region Login Providerlar
        //Task<TokenDto> LoginWithGoogleAccountAsync(GoogleLoginDto googleLoginDto);
        //Task<TokenDto> LoginWithFacebookAccountAsync(string returnUrl = "/");
        //Task<TokenDto> LoginWithTwitterAccountAsync(string returnUrl = "/");
        //Task<TokenDto> LoginWithMicrosoftAccountAsync(string returnUrl = "/");
        //Task<TokenDto> LoginWithTelegramAccountAsync(string returnUrl = "/");
        #endregion

        Task<Result> RegisterAsync(RegisterDto registerDto);
        Task<Result<TokenDto>> ConfirmEmailAsync(EmailConfirmationDto emailConfirmationDto);

        Task<Result<TokenDto>> LoginAsync(LoginDto loginDto);
        Task<Result<TokenDto>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
    }
}
