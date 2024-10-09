﻿using FluentValidation;
using Messenger.Application.DataTransferObjects.Auth;
using Messenger.Application.Helpers.PasswordHasher;
using Messenger.Application.Services.Email;
using Messenger.Application.Services.Token;
using Messenger.Application.Validators;
using Messenger.Domain.Entities;
using Messenger.Domain.Exceptions;
using Messenger.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

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

        //Todo Email tasdiqlashda code malum vaqt ichida kiritilishi kerak (masalan: 3:00)
        public async Task<TokenDto> ConfirmEmailAsync([FromForm]EmailConfirmationDto emailConfirmationDto)
        {
            var user = await _messengerDbContext.Users
                .FirstOrDefaultAsync(u => u.Email == emailConfirmationDto.Email);

            if(user.IsEmailConfirmed)
                throw new Exception("Email allaqachon tasdiqlangan, Login qiling!");

            if (user is null)
                throw new NotFoundException("Foydalanuvchi topilmadi.");

            if (user.ConfirmationCode != emailConfirmationDto.ConfirmationCode)
                throw new Exception("Tasdiqlash kodi noto'g'ri.");

            user.IsEmailConfirmed = true; // Tasdiqlangan foydalanuvchi holatini yangilash
            await _messengerDbContext.SaveChangesAsync();

            return await _tokenService.GenerateTokenAsync(user);
        }

        public async Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
            var validator = new LoginDtoValidator();
            var result = await validator.ValidateAsync(loginDto);

            if (!result.IsValid)
                throw new ValidationException("Model login qilish uchun yaroqsiz", result.Errors);

            var user = await _messengerDbContext.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user is null)
                throw new NotFoundException("Foydalanuvchi topilmadi.");

            if(!user.IsEmailConfirmed)
                throw new Exception("Email tasdiqlanmagan.");

            // Parolni tekshirish
            if (!_passwordHasher.Verify(user.PasswordHash, loginDto.Password, user.Salt))
                throw new ValidationException("Noto'g'ri parol.");

            user.RefreshToken = Guid.NewGuid().ToString();
            user.RefreshTokenExpireDate = DateTime.UtcNow.AddDays(30);
            await _messengerDbContext.SaveChangesAsync();

            return await _tokenService.GenerateTokenAsync(user);
        }

        public async Task<TokenDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto) 
            => await _tokenService.RefreshTokenAsync(refreshTokenDto);


        public async Task RegisterAsync(RegisterDto registerDto)
        {
            var validator = new RegisterDtoValidator();
            var result = await validator.ValidateAsync(registerDto);

            if (!result.IsValid)
                throw new ValidationException("Model ro'yhatdan o'tish uchun yaroqsiz",result.Errors);

            var existingUser = await _messengerDbContext.Users
                .FirstOrDefaultAsync(u => u.Email == registerDto.Email || u.PhoneNumber == registerDto.PhoneNumber);

            if (existingUser != null)
                throw new Exception("Ushbu email yoki phone number allaqachon ro'yxatdan o'tgan.");

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
                RefreshTokenExpireDate = DateTime.UtcNow.AddDays(30),
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

            await _emailService.SendEmailAsync(user.Email, user.FirstName, "Confirm Your Email", emailBody.ToString());
        }
    }
}
