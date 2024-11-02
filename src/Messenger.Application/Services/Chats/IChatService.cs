using Messenger.Application.DataTransferObjects.Chats;
using Messenger.Application.DataTransferObjects.Filters;
using Messenger.Application.DataTransferObjects.Users;
using Messenger.Domain.Entities;

namespace Messenger.Application.Services.Chats
{
    public interface IChatService
    {
        Task<ChatDetailsViewModel> CreateChannelAsync(ChannelCreationDto channelCreationDto); // kanal ochish
        Task<ChatDetailsViewModel> CreateGroupAsync(GroupCreationDto groupCreationDto);       // group ochish
        Task<ChatDetailsViewModel> GetOrCreatePrivateChatAsync(long userId);                  // men qaysidur userga yozmoqchi bo'lsam uni userid
                                                                                              // sini jo'nataman va ikkalamizga bitta chat ochiladi
        Task<ChatDetailsViewModel> GetChatAsync(long id);                                     // get by id
        Task<List<ChatViewModel>> SearchChatsAsync(string key);                                  // qidirishda
        Task<List<UserViewModel>> SearchUsersAsync(string key);                                  // qidirishda
        Task<List<ChatViewModel>> GetUserChatsAsync();                                        // qo'shilgan hamma chatlar
        Task<List<ChatViewModel>> GetOwnerChatsAsync();                                       // mualliflik chatlari
        Task<List<ChatViewModel>> GetAdminChatsAsync();                                       // admin bo'lgan chatlari
        Task<ChatDetailsViewModel> UpdateChatAsync(ChatModificationDto chatModificationDto);  // chat malumotlarini edit qilish
        Task<ChatViewModel> DeleteAsync(long id);                                             // delete qilish

        Task<List<Chat>> GetChatsAsync();                                                     // all chats - admin panel uchun
    }
}
