using Messenger.Application.DataTransferObjects;
using Messenger.Application.DataTransferObjects.Auth;
using Messenger.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Messenger.Application.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        public TokenService(IConfiguration configuration) 
            => _jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        
        public async Task<TokenDto> GenerateTokenAsync(User user)
        {
            // Token yaratish uchun kerakli ma'lumotlarni tayyorlash
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            // JWT maxfiy kaliti
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tokenning amal qilish muddati
            var accessTokenExpiration = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes);
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);

            // Token yaratish
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer, // Agar kerak bo'lsa, o'zgartiring
                audience: _jwtSettings.Audience, // Agar kerak bo'lsa, o'zgartiring
                claims: claims,
                expires: accessTokenExpiration,
                signingCredentials: creds
            );

            // Tokenni stringga aylantirish
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // TokenDto qaytarish
            return new TokenDto
            {
                AccessToken = tokenString,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpireDate = refreshTokenExpiration
            };
        }
    }
}
