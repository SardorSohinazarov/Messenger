using Messenger.Application.Models.DataTransferObjects.Chats;
using Messenger.Application.Models.DataTransferObjects.Users;
using Messenger.Application.Models.Results;
using Messenger.Application.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
            => _userService = userService;

        [HttpGet("search")]
        public async Task<IActionResult> SearchUsersAsync(string key)
        {
            var chats = await _userService.SearchUsersAsync(key);

            return Ok(chats);
        }

        [HttpGet]
        public async Task<Result<List<UserViewModel>>> GetAllAsync([FromQuery] UsersFilterModel usersFilterModel)
        {
            var users = await _userService.GetAllUsers(usersFilterModel);

            return users;
        }
    }
}
