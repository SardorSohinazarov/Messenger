using Messenger.Application.DataTransferObjects.Auth;
using Messenger.Application.DataTransferObjects.Auth.Google;

namespace Messenger.Application.Services.Auth.Google
{
    public interface IGoogleAuthService
    {
        Task<TokenDto> SignAsync(GoogleLoginDto googleLoginDto);
    }
}
