using Messenger.Application.Helpers.PasswordHasher;
using Messenger.Application.Models.DataTransferObjects.Auth;
using Messenger.Application.Models.DataTransferObjects.Auth.Google;
using Messenger.Application.Models.DataTransferObjects.CommonOptions;
using Messenger.Application.Models.Results;
using Messenger.Application.Services.Token;
using Messenger.Domain.Entities;
using Messenger.Domain.Enums;
using Messenger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Messenger.Application.Services.Auth.Google
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly MessengerDbContext _messengerDbContext;
        private readonly GoogleOAuthOptions _googleOAuthOptions;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<GoogleAuthService> _logger;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public GoogleAuthService(
            IConfiguration configuration,
            ILogger<GoogleAuthService> logger,
            IPasswordHasher passwordHasher,
            MessengerDbContext messengerDbContext,
            ITokenService tokenService)
        {
            _googleOAuthOptions = configuration.GetSection("GoogleOAuthOptions").Get<GoogleOAuthOptions>();
            _jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            _logger = logger;
            _passwordHasher = passwordHasher;
            _messengerDbContext = messengerDbContext;
            _tokenService = tokenService;
        }

        public async Task<Result<TokenDto>> SignAsync(GoogleLoginDto googleLoginDto)
        {
            Payload payload = new();

            try
            {
                payload = await ValidateAsync(googleLoginDto.IdToken, new ValidationSettings
                {
                    Audience = new[] { _googleOAuthOptions.ClientId }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new UnauthorizedAccessException(ex.Message, ex);
            }

            var existingUser = await _messengerDbContext.Users
                .FirstOrDefaultAsync(u => u.Email == payload.Email);

            if(existingUser is not null)
            {
                TokenDto token1 = await _tokenService.GenerateTokenAsync(existingUser);
                return Result<TokenDto>.Success(token1);
            }

            var user = await CreateNewUserAsync(payload);

            var token = await _tokenService.GenerateTokenAsync(user);
            return Result<TokenDto>.Success(token);
        }

        private async Task<User> CreateNewUserAsync(Payload payload)
        {
            var userName = payload.Email.Substring(0, payload.Email.IndexOf('@'));
            var user = new User
            {
                FirstName = payload.GivenName,
                LastName = payload.FamilyName,
                Email = payload.Email,
                //ProfilePicture = payload.Picture,
                //LoginProviderSubject = payload.Subject,
                UserName = userName,
                RefreshToken = Guid.NewGuid().ToString(),
                RefreshTokenExpireDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays),
                LoginProvider = ELoginProvider.Google
            };

            var entityEntry = await _messengerDbContext.Users.AddAsync(user);
            await _messengerDbContext.SaveChangesAsync();
            return entityEntry.Entity;
        }
    }
}
