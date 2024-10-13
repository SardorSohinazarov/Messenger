using Messenger.Application.DataTransferObjects.Chats;
using Messenger.Application.DataTransferObjects.Filters;
using Messenger.Application.Services.Chats;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatsController(IChatService chatService) 
            => _chatService = chatService;

        [HttpPost]
        public async Task<IActionResult> CreateChatAsync(ChatCreationDto chatCreationDto)
        {
            var chat = await _chatService.CreateChatAsync(chatCreationDto);

            return Ok(chat);
        }

        [HttpPost("filter")]
        public async Task<IActionResult> GetChatsAsync(ChatFilter chatFilter)
        {
            var chats = await _chatService.GetChatsAsync(chatFilter);

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

        [HttpGet("generate-link")]
        public async Task<IActionResult> GenerateChatLinkAsync(long id)
        {
            var link = await _chatService.CreateChatInviteLinkAsync(id);

            return Ok(link);
        }

        [HttpGet("join-chat")]
        public async Task<IActionResult> JoinChatAsync(string link)
        {
            var chat = await _chatService.JoinChatAsync(link);

            return Ok(chat);
        }
    }
}
