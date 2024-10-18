using Messenger.Application.DataTransferObjects.Auth;
using Messenger.Application.DataTransferObjects.Auth.Google;
using Messenger.Application.DataTransferObjects.CommonOptions;
using Messenger.Application.Helpers.PasswordHasher;
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

        public async Task<TokenDto> SignAsync(GoogleLoginDto googleLoginDto)
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
                return await _tokenService.GenerateTokenAsync(existingUser);

            var confirmationCode = new Random().Next(1000, 9999).ToString();
            var salt = Guid.NewGuid().ToString();
            var passwordHash = _passwordHasher.Encrypt(salt, salt);
            var refreshToken = Guid.NewGuid().ToString();

            var user = new User()
            {
                FirstName = payload.GivenName,
                LastName = payload.FamilyName,
                Email = payload.Email,
                IsEmailConfirmed = true,
                //ProfilePicture = payload.Picture,
                //LoginProviderSubject = payload.Subject,
                UserName = payload.Email,
                RefreshTokenExpireDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays),
                RefreshToken = Guid.NewGuid().ToString(),
                PasswordHash = passwordHash,
                Salt = salt,
                ConfirmationCode = confirmationCode,
                LoginProvider = ELoginProvider.Google
            };

            var entityEntry = await _messengerDbContext.Users.AddAsync(user);
            await _messengerDbContext.SaveChangesAsync();

            return await _tokenService.GenerateTokenAsync(entityEntry.Entity);
        }
    }
}
