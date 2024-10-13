using Messenger.Domain.Entities;

namespace Messenger.Application.Services.Messages
{
    public interface IMessagesService
    {
        Task<Message> CreateMessageAsync(Message message);
        Task<List<Message>> GetMessagesAsync();
        Task<List<Message>> GetMessagesAsync(long chatId);
        Task<Message> GetMessageByIdAsync(Guid id);
        Task<Message> UpdateMessageAsync(Message message);
        Task<Message> DeleteMessageAsync(Guid id);
    }
}
