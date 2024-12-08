using Messenger.UI.Models.Auth;

namespace Messenger.UI.Services.Auth
{
    public interface IAuthService
    {
        Task<TokenDto> LoginAsync(LoginDto loginDto);
    }
}
