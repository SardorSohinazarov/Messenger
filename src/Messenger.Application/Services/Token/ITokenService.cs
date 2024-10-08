﻿using Messenger.Application.DataTransferObjects.Auth;
using Messenger.Domain.Entities;

namespace Messenger.Application.Services.Token
{
    public interface ITokenService
    {
        Task<TokenDto> GenerateTokenAsync(User user);
    }
}