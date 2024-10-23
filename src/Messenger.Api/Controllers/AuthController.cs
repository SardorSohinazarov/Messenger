using Messenger.Application.DataTransferObjects.Auth;
using Messenger.Application.DataTransferObjects.Auth.Google;
using Messenger.Application.DataTransferObjects.Auth.UserProfiles;
using Messenger.Application.Services.Auth;
using Messenger.Application.Services.Auth.Google;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IGoogleAuthService _googleAuthService;

        public AuthController(
            IAuthService authService,
            IGoogleAuthService googleAuthService)
        {
            _authService = authService;
            _googleAuthService = googleAuthService;
        }

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
            var token = await _googleAuthService.SignAsync(googleLoginDto);

            return Ok(token);
        }

        //profile
        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfileAsync()
        {
            var profile = await _authService.GetUserProfileAsync();

            return Ok(profile);
        }
        
        [HttpGet("profile/{id}")]
        public async Task<IActionResult> GetUserProfileAsync(long id)
        {
            var profile = await _authService.GetUserProfileAsync(id);

            return Ok(profile);
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfileAsync(UserProfileModificationDto userProfileModificationDto)
        {
            var profile = await _authService.UpdateUserProfileAsync(userProfileModificationDto);

            return Ok(profile);
        }

        [HttpDelete("delete-profile")]
        public async Task<IActionResult> DeleteProfileAsync()
        {
            await _authService.DeleteUserProfileAsync();

            return Ok();
        }

        [HttpGet("confirm-delete-profile")]
        public async Task<IActionResult> ConfirmDeleteProfileAsync([FromQuery] EmailConfirmationDto emailConfirmationDto)
        {
            var userProfile = await _authService.ConfirmDeleteProfileAsync(emailConfirmationDto);

            return Ok(userProfile);
        }
    }
}
