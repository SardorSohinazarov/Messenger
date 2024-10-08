using FluentValidation;
using Messenger.Application.DataTransferObjects.Auth;
using Messenger.Application.Helpers;
using Messenger.Application.Services.Email;
using Messenger.Application.Services.Token;
using Messenger.Application.Validators;
using Messenger.Domain.Entities;
using Messenger.Domain.Exceptions;
using Messenger.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly MessengerDbContext _messengerDbContext;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(
            MessengerDbContext messengerDbContext,
            IPasswordHasher passwordHasher,
            IEmailService emailService,
            ITokenService tokenService,
            IHttpContextAccessor httpContextAccessor)
        {
            _messengerDbContext = messengerDbContext;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TokenDto> ConfirmEmailAsync([FromForm]EmailConfirmationDto emailConfirmationDto)
        {
            // Foydalanuvchini email orqali topish
            var user = await _messengerDbContext.Users
                .FirstOrDefaultAsync(u => u.Email == emailConfirmationDto.Email);

            if (user is null)
                throw new NotFoundException("Foydalanuvchi topilmadi.");

            // Tasdiqlash kodini tekshirish
            if (user.ConfirmationCode != emailConfirmationDto.ConfirmationCode)
                throw new Exception("Tasdiqlash kodi noto'g'ri.");

            // Emailni tasdiqlash
            user.IsEmailConfirmed = true; // Tasdiqlangan foydalanuvchi holatini yangilash
            await _messengerDbContext.SaveChangesAsync();

            // Token yaratish
            return await _tokenService.GenerateTokenAsync(user); // Token yaratish funksiyasini chaqirish
        }

        public async Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
            // Foydalanuvchini email bo'yicha qidirish
            var user = await _messengerDbContext.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user is null)
                throw new NotFoundException("Foydalanuvchi topilmadi.");

            if(!user.IsEmailConfirmed)
                throw new Exception("Email tasdiqlanmagan.");

            // Parolni tekshirish
            if (!_passwordHasher.Verify(user.PasswordHash, loginDto.Password, user.Salt))
                throw new ValidationException("Noto'g'ri parol.");

            // Token yaratish
            return await _tokenService.GenerateTokenAsync(user);
        }

        public Task<TokenDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            throw new NotImplementedException();
        }

        public async Task RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _messengerDbContext.Users
                .FirstOrDefaultAsync(u => u.Email == registerDto.Email || u.PhoneNumber == registerDto.PhoneNumber);

            if (existingUser != null)
                throw new Exception("Ushbu email yoki phone number allaqachon ro'yxatdan o'tgan.");

            var validator = new RegisterDtoValidator();
            var result = await validator.ValidateAsync(registerDto);

            if (!result.IsValid)
                throw new ValidationException("Model ro'yhatdan o'tish uchun yaroqsiz",result.Errors);

            var confirmationCode = new Random().Next(1000, 9999).ToString();
            var salt = Guid.NewGuid().ToString();
            var passwordHash = _passwordHasher.Encrypt(registerDto.Password, salt);
            var refreshToken = Guid.NewGuid().ToString();

            var user = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
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

            var confirmationLink = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/Auth/confirm-email?Email={Uri.EscapeDataString(user.Email)}&ConfirmationCode={confirmationCode}";
            var emailBody = $"Click the link to confirm your email: <a href='{confirmationLink}'>Confirm Email</a>";
            await _emailService.SendEmailAsync(user.Email, user.FirstName, "Confirm Your Email", emailBody);
        }
    }
}
