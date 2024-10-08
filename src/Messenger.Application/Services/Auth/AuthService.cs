using Messenger.Application.DataTransferObjects.Auth;
using Messenger.Application.Helpers;
using Messenger.Application.Services.Email;
using Messenger.Domain.Entities;
using Messenger.Infrastructure.Persistence;
using System;

namespace Messenger.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly MessengerDbContext _messengerDbContext;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailService _emailService;

        public AuthService(
            MessengerDbContext messengerDbContext,
            IPasswordHasher passwordHasher,
            IEmailService emailService)
        {
            _messengerDbContext = messengerDbContext;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
        }

        public Task<TokenDto> ConfirmEmailAsync(EmailConfirmationDto emailConfirmationDto)
        {
            throw new NotImplementedException();
        }

        public Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        public Task<TokenDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            throw new NotImplementedException();
        }

        public async Task RegisterAsync(RegisterDto registerDto)
        {
            var confirmationCode = new Random().Next(1000, 9999).ToString();
            var salt = Guid.NewGuid().ToString();
            var passwordHash = _passwordHasher.Encrypt(registerDto.Password, salt);
            var refreshToken = Guid.NewGuid().ToString();

            var user = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber,
                LanguageCode = null,
                ConfirmationCode = confirmationCode,
                Salt = salt,
                PasswordHash = passwordHash,
                RefreshToken = refreshToken,
                RefreshTokenExpireDate = DateTime.UtcNow.AddMinutes(5),
            };

            await _messengerDbContext.Users.AddAsync(user);
            await _messengerDbContext.SaveChangesAsync();

            var confirmationLink = "https://www.youtube.com/";
            var emailBody = $"Click the link to confirm your email: <a href='{confirmationLink}'>Confirm Email</a>";
            var from = "messenger@gmail.com";
            await _emailService.SendEmailAsync(user.Email, user.FirstName, "Confirm Your Email", emailBody);
        }
    }
}
