﻿namespace Messenger.Application.Models.DataTransferObjects.Auth
{
    public class RefreshTokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
