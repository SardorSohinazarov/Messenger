using Messenger.Application.DataTransferObjects.Auth;
using Messenger.Application.DataTransferObjects.CommonOptions;
using Messenger.Domain.Entities;
using Messenger.Domain.Exceptions;
using Messenger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
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
        private readonly MessengerDbContext _messengerDbContext;
        public TokenService(
            IConfiguration configuration,
            MessengerDbContext messengerDbContext)
        {
            _jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            _messengerDbContext = messengerDbContext;
        }

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

        public async Task<TokenDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(refreshTokenDto.AccessToken);
            if (principal == null)
                throw new UnauthorizedAccessException("Invalid access token.");

            var userId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new UnauthorizedAccessException("Invalid token claims.");

            var user = await _messengerDbContext.Users
                .FirstOrDefaultAsync(u => u.Id == int.Parse(userId));

            if (user.RefreshToken != refreshTokenDto.RefreshToken || user.RefreshTokenExpireDate < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");

            if (user == null)
                throw new NotFoundException("User not found.");

            return await GenerateTokenAsync(user);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                ValidateLifetime = false, // We are not validating the token's expiration here
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            try
            {
                var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
