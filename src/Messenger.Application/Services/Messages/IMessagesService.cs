using Messenger.Application.DataTransferObjects.Messages;
using Messenger.Domain.Entities;

namespace Messenger.Application.Services.Messages
{
    public interface IMessagesService
    {
        Task<MessageViewModel> CreateMessageAsync(MessageCreationDto messageCreationDto);
        Task<List<Message>> GetMessagesAsync();
        Task<List<MessageViewModel>> GetMessagesAsync(long chatId);
        Task<MessageViewModel> GetMessageByIdAsync(Guid id);
        Task<MessageViewModel> UpdateMessageAsync(MessageModificationDto messageModificationDto);
        Task<MessageViewModel> DeleteMessageAsync(Guid id);
    }
}
