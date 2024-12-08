using Messenger.UI.Models.Auth;
using System.Net.Http.Json;

namespace Messenger.UI.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient) 
            => _httpClient = httpClient;

        public async Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadFromJsonAsync<TokenDto>();
                return token ?? throw new InvalidOperationException("Token not received");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Login failed: {response.StatusCode}, {errorContent}");
        }
    }
}
