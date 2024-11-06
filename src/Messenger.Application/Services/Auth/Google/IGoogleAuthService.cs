using Messenger.Application.Models.DataTransferObjects.Auth;
using Messenger.Application.Models.DataTransferObjects.Auth.Google;
using Messenger.Application.Models.Results;

namespace Messenger.Application.Services.Auth.Google
{
    public interface IGoogleAuthService
    {
        Task<Result<TokenDto>> SignAsync(GoogleLoginDto googleLoginDto);
    }
}
