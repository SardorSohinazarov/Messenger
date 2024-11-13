using Messenger.Application.Models.DataTransferObjects.Messages;
using Messenger.Application.Models.Results;
using Messenger.Domain.Entities;

namespace Messenger.Application.Services.Messages
{
    public interface IMessagesService
    {
        Task<Result<MessageViewModel>> CreateMessageAsync(MessageCreationDto messageCreationDto);
        Task<Result<List<Message>>> GetMessagesAsync(MessagesPaginationSelectionDto messagesPaginationSelectionDto);
        Task<Result<List<MessageViewModel>>> GetMessagesAsync(MessagesPaginationSelectionByChatDto messagesPaginationSelectionByChatDto);
        Task<Result<MessageViewModel>> GetMessageByIdAsync(Guid id);
        Task<Result<MessageViewModel>> UpdateMessageAsync(MessageModificationDto messageModificationDto);
        Task<Result<MessageViewModel>> DeleteMessageAsync(Guid id);
    }
}
