using Messenger.Application.DataTransferObjects.Auth;
using Messenger.Application.Services.Auth;
using Messenger.Application.Services.Email;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public AuthController(IAuthService authService, IEmailService emailService)
        {
            _authService = authService;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            await _authService.RegisterAsync(registerDto);

            return Ok();
        }
        
        [HttpPost("email")]
        public async Task<IActionResult> Register()
        {
            await _emailService.SendEmailAsync("sardorstudent0618@gmail.com", "isoqovxudoyorbek@gmail.com", "Qales", "Nima gap!");
            return Ok();
        }
    }
}
