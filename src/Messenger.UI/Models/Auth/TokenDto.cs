using System.Text.Json.Serialization;

namespace Messenger.UI.Models.Auth
{
    public class TokenDto
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
        [JsonPropertyName("refreshTokenExpireDate")]
        public DateTime RefreshTokenExpireDate { get; set; }
    }
}
