using Messenger.Application.DataTransferObjects;
using Messenger.Application.DataTransferObjects.Auth;
using Messenger.Application.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) 
            => _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            await _authService.RegisterAsync(registerDto);

            return Ok();
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery]EmailConfirmationDto emailConfirmationDto)
        {
            var token = await _authService.ConfirmEmailAsync(emailConfirmationDto);

            return Ok(token);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var token = await _authService.LoginAsync(loginDto);

            return Ok(token);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            var token = await _authService.RefreshTokenAsync(refreshTokenDto);

            return Ok(token);
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> LoginWithGoogleAccountAsync(GoogleLoginDto googleLoginDto)
        {
            var token = await _authService.LoginWithGoogleAccountAsync(googleLoginDto);

            return Ok(token);
        }
    }
}
