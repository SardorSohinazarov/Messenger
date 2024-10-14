using Messenger.Application.DataTransferObjects.Chats;
using Messenger.Application.DataTransferObjects.Filters;
using Messenger.Domain.Entities;

namespace Messenger.Application.Services.Chats
{
    public interface IChatService
    {
        Task<ChatDetailsViewModel> CreateChatAsync(ChatCreationDto chatCreationDto);
        Task<ChatDetailsViewModel> GetOrCreatePrivateChat(long userId);
        Task<List<Chat>> GetChatsAsync();
        Task<ChatDetailsViewModel> GetChatAsync(long id);
        Task<List<ChatViewModel>> GetChatsAsync(ChatFilter filter);
        Task<ChatDetailsViewModel> UpdateChatAsync(ChatModificationDto chatModificationDto);
        Task<ChatViewModel> DeleteAsync(long id);
    }
}
