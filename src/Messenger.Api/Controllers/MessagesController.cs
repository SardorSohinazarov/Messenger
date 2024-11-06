using Messenger.Application.Models.DataTransferObjects.Messages;
using Messenger.Application.Services.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IMessagesService _messagesService;

        public MessagesController(IMessagesService messagesService) 
            => _messagesService = messagesService;

        [HttpGet("all")]
        public async Task<IActionResult> GetMessagesAsync()
        {
            var messages = await _messagesService.GetMessagesAsync();

            return Ok(messages);
        }

        [HttpGet("chat/{chatId}")]
        public async Task<IActionResult> GetMessagesAsync(long chatId)
        {
            var messages = await _messagesService.GetMessagesAsync(chatId);

            return Ok(messages);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessageByIdAsync(Guid id)
        {
            var message = await _messagesService.GetMessageByIdAsync(id);

            return Ok(message);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessageAsync(MessageCreationDto messageCreationDto)
        {
            var message = await _messagesService.CreateMessageAsync(messageCreationDto);

            return Ok(message);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMessageAsync(MessageModificationDto messageModificationDto)
        {
            var message = await _messagesService.UpdateMessageAsync(messageModificationDto);

            return Ok(message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessageAsync(Guid id)
        {
            var message = await _messagesService.DeleteMessageAsync(id);

            return Ok(message);
        }
    }
}
