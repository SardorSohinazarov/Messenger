using Messenger.Application.DataTransferObjects.Auth;
using Messenger.Domain.Entities;
using System.Security.Claims;

namespace Messenger.Application.Services.Token
{
    public interface ITokenService
    {
        Task<TokenDto> GenerateTokenAsync(User user);
        Task<TokenDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
    }
}
