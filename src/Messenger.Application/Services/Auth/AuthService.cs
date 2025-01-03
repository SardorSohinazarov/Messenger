﻿using Messenger.Application.Helpers.PasswordHasher;
using Messenger.Application.Services.Email;
using Messenger.Application.Services.Token;
using Messenger.Domain.Entities;
using Messenger.Domain.Exceptions;
using Messenger.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using ValidationException = FluentValidation.ValidationException;
using Messenger.Application.Validators.Auth;
using Messenger.Domain.Enums;
using Microsoft.Extensions.Caching.Memory;
using Messenger.Application.Models.DataTransferObjects.Auth;
using Messenger.Application.Models.Results;

namespace Messenger.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly MessengerDbContext _messengerDbContext;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _memoryCache;

        public AuthService(
            MessengerDbContext messengerDbContext,
            IPasswordHasher passwordHasher,
            IEmailService emailService,
            ITokenService tokenService,
            IHttpContextAccessor httpContextAccessor,
            IMemoryCache memoryCache)
        {
            _messengerDbContext = messengerDbContext;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
            _memoryCache = memoryCache;
        }

        //Todo email html codeni boshqa joydan o'qishligi kerak M:bazadan
        public async Task<Result> RegisterAsync(RegisterDto registerDto)
        {
            var validator = new RegisterDtoValidator();
            var result = await validator.ValidateAsync(registerDto);

            if (!result.IsValid)
                throw new ValidationException("Model ro'yhatdan o'tish uchun yaroqsiz",result.Errors);

            #region Optimallashtirish kerak : Telefon nomer yoki Email yoki UserName band qilinganmi yo'qmi tekshirish kerak 
            var userNameOrPhoneOrEmailExist = await _messengerDbContext.Users
                .AnyAsync(x => x.Email == registerDto.Email
                            || x.PhoneNumber == registerDto.PhoneNumber);

            if (userNameOrPhoneOrEmailExist)
                throw new ValidationException("Telefon nomer yoki Email allaqachon band qilingan.");
            #endregion

            var confirmationCode = new Random().Next(1000, 9999).ToString();
            var salt = Guid.NewGuid().ToString();
            var passwordHash = _passwordHasher.Encrypt(registerDto.Password, salt);
            var refreshToken = Guid.NewGuid().ToString();
            var userName = registerDto.Email.Substring(0, registerDto.Email.Length - 10); // Username uchun emaildan qirqib olamiz
                                                                                          // ( email: sardorstudent0618@gmail.com -> username: sardorstudent0618)
            var user = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = userName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                LanguageCode = null,
                IsEmailConfirmed = false,
                Salt = salt,
                PasswordHash = passwordHash,
                RefreshToken = refreshToken,
                RefreshTokenExpireDate = DateTime.UtcNow.AddDays(30),
                LoginProvider = ELoginProvider.Email
            };

            await _messengerDbContext.Users.AddAsync(user);
            await _messengerDbContext.SaveChangesAsync();

            var confirmationLink = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/Auth/confirm-email?Email={Uri.EscapeDataString(user.Email)}&ConfirmationCode={confirmationCode}";
            var emailBody = new StringBuilder(@"""<!DOCTYPE html>
                <html lang=""uz"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                            background-color: #f9f9f9;
                            margin: 0;
                            padding: 20px;
                        }
                        .container {
                            background-color: #ffffff;
                            border-radius: 5px;
                            padding: 20px;
                            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                            text-align: center; /* O'rtaga joylash uchun */
                        }
                        h1 {
                            color: #333333;
                        }
                        p {
                            color: #555555;
                            line-height: 1.6;
                        }
                        .button {
                            background-color: #007bff;
                            color: #ffffff;
                            padding: 10px 20px;
                            text-decoration: none;
                            border-radius: 5px;
                            display: inline-block;
                            margin-top: 20px;
                        }
                        .code {
                            font-size: 36px; /* Tasdiqlash kodining o'lchami */
                            font-weight: bold; /* Katta harfda bo'lishi */
                            color: #007bff; /* Tasdiqlash kodi uchun rang */
                            padding: 20px;
                            border: 2px solid #007bff; /* Tasdiqlash kodi uchun chegara */
                            display: inline-block; /* Hujjatning o'ziga xos joyini saqlash */
                            margin-top: 30px; /* O'rta joylash uchun yuqoridan bo'shliq */
                        }
                        .footer {
                            margin-top: 30px;
                            font-size: 12px;
                            color: #888888;
                            text-align: center;
                        }
                    </style>
                </head>
                <body>

                <div class=""container"">
                    <h1>Hisobingizni tasdiqlash!</h1>
                    <p>Salom <strong>FOYDALANUVCHIISMI</strong>,</p>
                    <p>Ro'yxatdan o'tganingiz uchun rahmat! Biz xizmatlarimizdan foydalanishni boshlashdan oldin, elektron pochtangizni tasdiqlashingiz kerak.</p>
    
                    <p>Quyidagi tasdiqlash kodini kiriting:</p>
                    <div class=""code"">KOD</div> <!-- Tasdiqlash kodi -->
    
                    <p>Yoki tasdiqlash uchun quyidagi tugmani bosing:</p>
                    <a href=""LINK"" class=""button"">Tasdiqlash uchun boshing</a>
    
                    <p>Biz bilan qoling va ajoyib imkoniyatlardan foydalaning!</p>
                </div>

                <div class=""footer"">
                    <p>Yana bir savolingiz bo'lsa, biz bilan bog'laning.</p>
                    <p>Bizning manzilimiz: sardorstudent0618@gmail.com</p>
                </div>

                </body>
                </html>""");

            emailBody.Replace("KOD", confirmationCode); // Tasdiqlash kodi
            emailBody.Replace("LINK", confirmationLink); // Tasdiqlash linki
            emailBody.Replace("FOYDALANUVCHIISMI", user.FirstName); // Foydalanuvchi ismi

            _memoryCache.Set(
                key: registerDto.Email,
                value: confirmationCode,
                options: new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(3),
                    }
                );

            await _emailService.SendEmailAsync(user.Email, user.FirstName, "Confirm Your Email", emailBody.ToString());

            return Result.Success("Emailga code jo'natildi.");
        }
        
        public async Task<Result<TokenDto>> ConfirmEmailAsync([FromForm]EmailConfirmationDto emailConfirmationDto)
        {
            var validator = new EmailConfirmationDtoValidator();
            var result = await validator.ValidateAsync(emailConfirmationDto);

            if (!result.IsValid)
                throw new ValidationException("Model email tasdiqlash uchun yaroqsiz", result.Errors);

            var emailConfiramtionCache = _memoryCache.Get(emailConfirmationDto.Email) as string;
            if (emailConfiramtionCache == null)
                throw new ValidationException("Kodni yaroqlilik muddati tugagan.");

            if (emailConfiramtionCache != emailConfirmationDto.ConfirmationCode)
                throw new ValidationException("Tasdiqlash kodi noto'g'ri.");

            var user = await _messengerDbContext.Users
                .FirstOrDefaultAsync(u => u.Email == emailConfirmationDto.Email);

            if (user is null)
                throw new NotFoundException("Foydalanuvchi topilmadi.");
         
            if(!user.IsEmailConfirmed.HasValue || user.IsEmailConfirmed.Value)
                throw new Exception("Email allaqachon tasdiqlangan, Login qiling!");

            user.IsEmailConfirmed = true;
            await _messengerDbContext.SaveChangesAsync();

            var token = await _tokenService.GenerateTokenAsync(user);

            return Result<TokenDto>.Success(token);
        }


        public async Task<Result<TokenDto>> LoginAsync(LoginDto loginDto)
        {
            var validator = new LoginDtoValidator();
            var result = await validator.ValidateAsync(loginDto);

            if (!result.IsValid)
                throw new ValidationException("Model login qilish uchun yaroqsiz", result.Errors);

            var user = await _messengerDbContext.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user is null)
                throw new NotFoundException("Foydalanuvchi topilmadi.");

            if (user.LoginProvider != ELoginProvider.Email)
                throw new NotFoundException($"Siz email tasdiqlash orqali ro'yxatdan o'tmagansiz, " +
                                            $"{user.LoginProvider} orqali login qiling!");

            if (!user.IsEmailConfirmed.Value)
                throw new ForbiddenException("Email tasdiqlanmagan.");

            if (!_passwordHasher.Verify(user.PasswordHash, loginDto.Password, user.Salt))
                throw new ValidationException("Noto'g'ri parol.");

            user.RefreshToken = Guid.NewGuid().ToString();
            user.RefreshTokenExpireDate = DateTime.UtcNow.AddDays(30);
            await _messengerDbContext.SaveChangesAsync();

            var token = await _tokenService.GenerateTokenAsync(user);

            return Result<TokenDto>.Success(token);
        }

        public async Task<Result<TokenDto>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            var validator = new RefreshTokenDtoValidator();
            var result = await validator.ValidateAsync(refreshTokenDto);

            if (!result.IsValid)
                throw new ValidationException("Refresh token yangilash uchun yaroqsiz", result.Errors);

            var refreshToken = await _tokenService.RefreshTokenAsync(refreshTokenDto);

            return Result<TokenDto>.Success(refreshToken);
        }
    }
}
