using Messenger.Application.DataTransferObjects.Chats;
using Messenger.Application.DataTransferObjects.Filters;
using Messenger.Domain.Entities;

namespace Messenger.Application.Services.Chats
{
    public interface IChatService
    {
        Task<ChatDetailsViewModel> CreateChatAsync(ChatCreationDto chatCreationDto);          // keraksiz
        Task<ChatDetailsViewModel> CreateChannelAsync(ChannelCreationDto channelCreationDto); // kanal ochish
        Task<ChatDetailsViewModel> CreateGroupAsync(GroupCreationDto groupCreationDto);       // group ochish
        Task<ChatDetailsViewModel> GetOrCreatePrivateChat(long userId);                       // men qaysidur userga yozmoqchi bo'lsam uni userid
                                                                                              // sini jo'nataman va ikkalamizga bitta chat ochiladi
        Task<List<Chat>> GetChatsAsync();                                                     // all chats - admin panel uchun
        Task<ChatDetailsViewModel> GetChatAsync(long id);                                     // get by id
        Task<List<ChatViewModel>> GetChatsAsync(ChatFilter filter);                           // qidirishda filterlashda
        Task<List<ChatViewModel>> GetOwnerChatsAsync();
        Task<List<ChatViewModel>> GetAdminChatsAsync();
        Task<ChatDetailsViewModel> UpdateChatAsync(ChatModificationDto chatModificationDto);  // chat malumotlarini edit qilish
        Task<ChatViewModel> DeleteAsync(long id);                                             // delete qilish
    }
}
