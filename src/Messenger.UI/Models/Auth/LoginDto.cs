using System.Text.Json.Serialization;

namespace Messenger.UI.Models.Auth
{
    public class LoginDto
    {
        [JsonPropertyName("password")]
        public string Password { get; set; }  // Foydalanuvchi paroli
        [JsonPropertyName("email")]
        public string Email { get; set; }  // Foydalanuvchi
    }
}
