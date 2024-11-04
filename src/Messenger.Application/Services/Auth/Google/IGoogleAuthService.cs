using Messenger.Application.Common.Results;
using Messenger.Application.DataTransferObjects.Auth;
using Messenger.Application.DataTransferObjects.Auth.Google;

namespace Messenger.Application.Services.Auth.Google
{
    public interface IGoogleAuthService
    {
        Task<Result<TokenDto>> SignAsync(GoogleLoginDto googleLoginDto);
    }
}
