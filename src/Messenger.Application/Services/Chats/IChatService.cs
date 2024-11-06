using Messenger.Application.DataTransferObjects.Filters;
using Messenger.Application.Models.DataTransferObjects.Chats;
using Messenger.Application.Models.DataTransferObjects.Users;
using Messenger.Application.Models.Results;
using Messenger.Domain.Entities;

namespace Messenger.Application.Services.Chats
{
    public interface IChatService
    {
        Task<Result<ChatDetailsViewModel>> CreateChannelAsync(ChannelCreationDto channelCreationDto); // kanal ochish
        Task<Result<ChatDetailsViewModel>> CreateGroupAsync(GroupCreationDto groupCreationDto);       // group ochish
        Task<Result<ChatDetailsViewModel>> GetOrCreatePrivateChatAsync(long userId);                  // men qaysidur userga yozmoqchi bo'lsam uni userid
                                                                                              // sini jo'nataman va ikkalamizga bitta chat ochiladi
        Task<Result<ChatDetailsViewModel>> GetChatAsync(long id);                                     // get by id
        Task<Result<List<ChatViewModel>>> SearchChatsAsync(string key);                                  // qidirishda
        Task<Result<List<UserViewModel>>> SearchUsersAsync(string key);                                  // qidirishda
        Task<Result<List<ChatViewModel>>> GetUserChatsAsync();                                        // qo'shilgan hamma chatlar
        Task<Result<List<ChatViewModel>>> GetOwnerChatsAsync();                                       // mualliflik chatlari
        Task<Result<List<ChatViewModel>>> GetAdminChatsAsync();                                       // admin bo'lgan chatlari
        Task<Result<ChatDetailsViewModel>> UpdateChatAsync(ChatModificationDto chatModificationDto);  // chat malumotlarini edit qilish
        Task<Result<ChatViewModel>> DeleteAsync(long id);                                             // delete qilish

        Task<Result<List<Chat>>> GetChatsAsync();                                                     // all chats - admin panel uchun
    }
}
