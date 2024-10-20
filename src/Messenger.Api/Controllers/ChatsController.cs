using Messenger.Application.DataTransferObjects.Chats;
using Messenger.Application.DataTransferObjects.Filters;
using Messenger.Application.Services.Chats;
using Messenger.Infrastructure.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;

namespace Messenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatsController(IChatService chatService) 
            => _chatService = chatService;

        [HttpPost("private")]
        public async Task<IActionResult> GetOrCreatePrivateChatAsync(long userId)
        {
            var chat = await _chatService.GetOrCreatePrivateChat(userId);

            return Ok(chat);
        }

        [HttpPost("group")]
        public async Task<IActionResult> CreatGroupChatAsync(GroupCreationDto groupCreationDto)
        {
            var chat = await _chatService.CreateGroupAsync(groupCreationDto);

            return Ok(chat);
        }

        [HttpPost("channel")]
        public async Task<IActionResult> CreateChannelChatAsync(ChannelCreationDto channelCreationDto)
        {
            var chat = await _chatService.CreateChannelAsync(channelCreationDto);

            return Ok(chat);
        }

        [HttpPost("filter")]
        public async Task<IActionResult> GetChatsAsync(ChatFilter chatFilter)
        {
            var chats = await _chatService.GetChatsAsync(chatFilter);

            return Ok(chats);
        }

        [HttpGet("admin")]
        public async Task<IActionResult> GetAdminChatsAsync()
        {
            var chats = await _chatService.GetAdminChatsAsync();

            return Ok(chats);
        }

        [HttpGet("owner")]
        public async Task<IActionResult> GetOwnerChatsAsync()
        {
            var chats = _chatService.GetOwnerChatsAsync();

            return Ok(chats);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetChatsAsync()
        {
            var chats = await _chatService.GetChatsAsync();

            return Ok(chats);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChatAsync(long id)
        {
            var chat = await _chatService.GetChatAsync(id);

            return Ok(chat);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateChatAsync(ChatModificationDto chatModificationDto)
        {
            var chat = await _chatService.UpdateChatAsync(chatModificationDto);

            return Ok(chat);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteChatAsync(long id)
        {
            var chat = await _chatService.DeleteAsync(id);

            return Ok(chat);
        }
    }
}
