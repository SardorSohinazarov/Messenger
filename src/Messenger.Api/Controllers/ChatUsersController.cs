using Messenger.Application.DataTransferObjects.ChatUsers;
using Messenger.Application.Services.ChatUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatUsersController : ControllerBase
    {
        private readonly IChatUserService _chatUserService;

        public ChatUsersController(IChatUserService chatUserService)
        {
            _chatUserService = chatUserService;
        }

        [HttpGet("join")]
        public async Task<IActionResult> JoinChatAsync(long id)
        {
            var chat = await _chatUserService.JoinChatAsync(id);

            return Ok(chat);
        }

        [HttpGet("join-link")]
        public async Task<IActionResult> JoinChatAsync(string link)
        {
            var chat = await _chatUserService.JoinChatAsync(link);

            return Ok(chat);
        }

        [HttpGet("leave")]
        public async Task<IActionResult> LeaveChatAsync(long chatId)
        {
            await _chatUserService.LeaveChatAsync(chatId);

            return Ok();
        }

        [HttpPost("block")]
        public async Task<IActionResult> BlokChatUserAsync(ChatUserDto chatUserDto)
        {
            var chat = await _chatUserService.BlokChatUserAsync(chatUserDto);

            return Ok(chat);
        }

        [HttpPost("unblock")]
        public async Task<IActionResult> UnBlokChatUserAsync(ChatUserDto chatUserDto)
        {
            var chat = await _chatUserService.UnBlokChatUserAsync(chatUserDto);

            return Ok(chat);
        }

        [HttpGet("invite-link")]
        public async Task<IActionResult> CreateChatInviteLinkAsync(long chatId)
        {
            var link = await _chatUserService.CreateChatInviteLinkAsync(chatId);

            return Ok(link);
        }
    }
}
