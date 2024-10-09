﻿using Messenger.Application.DataTransferObjects;
using Messenger.Application.DataTransferObjects.Auth;

namespace Messenger.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<TokenDto> LoginAsync(LoginDto loginDto);
        Task<TokenDto> LoginWithGoogleAccountAsync(GoogleLoginDto googleLoginDto);
        //Task<TokenDto> LoginWithFacebookAccountAsync(string returnUrl = "/");
        //Task<TokenDto> LoginWithTwitterAccountAsync(string returnUrl = "/");
        //Task<TokenDto> LoginWithMicrosoftAccountAsync(string returnUrl = "/");
        //Task<TokenDto> LoginWithTelegramAccountAsync(string returnUrl = "/");

        Task RegisterAsync(RegisterDto registerDto);
        Task<TokenDto> ConfirmEmailAsync(EmailConfirmationDto emailConfirmationDto);

        Task<TokenDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
    }
}
