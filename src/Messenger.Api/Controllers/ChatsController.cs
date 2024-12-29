using Messenger.Application.Models.DataTransferObjects.Chats;
using Messenger.Application.Services.Chats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var chat = await _chatService.GetOrCreatePrivateChatAsync(userId);

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

        [HttpGet("search")]
        public async Task<IActionResult> SearchChatsAsync(string key)
        {
            var chats = await _chatService.SearchChatsAsync(key);

            return Ok(chats);
        }  
      
        [HttpGet]
        public async Task<IActionResult> GetUserChatsAsync()
        {
            var chats = await _chatService.GetUserChatsAsync();

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
            var chats = await _chatService.GetOwnerChatsAsync();

            return Ok(chats);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllChatsAsync([FromQuery] ChatsPaginationSelectionDto chatsPaginationSelectionDto)
        {
            var chats = await _chatService.GetChatsAsync(chatsPaginationSelectionDto);

            return Ok(chats);
        }

        [HttpGet("{id:long}")]
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

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteChatAsync(long id)
        {
            var chat = await _chatService.DeleteAsync(id);

            return Ok(chat);
        }
    }
}
