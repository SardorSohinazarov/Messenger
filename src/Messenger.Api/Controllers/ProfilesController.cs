using Messenger.Application.DataTransferObjects.Auth.UserProfiles;
using Messenger.Application.DataTransferObjects.Auth;
using Messenger.Application.Services.Profiles;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfilesController(IProfileService profileService) 
            => _profileService = profileService;

        [HttpGet("me")]
        public async Task<IActionResult> GetUserProfileAsync()
        {
            var profile = await _profileService.GetUserProfileAsync();

            return Ok(profile);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetUserProfileAsync(long id)
        {
            var profile = await _profileService.GetUserProfileAsync(id);

            return Ok(profile);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfileAsync(UserProfileModificationDto userProfileModificationDto)
        {
            var profile = await _profileService.UpdateUserProfileAsync(userProfileModificationDto);

            return Ok(profile);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProfileAsync()
        {
            await _profileService.DeleteUserProfileAsync();

            return Ok();
        }

        [HttpGet("confirm-delete-profile")]
        public async Task<IActionResult> ConfirmDeleteProfileAsync([FromQuery] EmailConfirmationDto emailConfirmationDto)
        {
            var userProfile = await _profileService.ConfirmDeleteProfileAsync(emailConfirmationDto);

            return Ok(userProfile);
        }
    }
}
