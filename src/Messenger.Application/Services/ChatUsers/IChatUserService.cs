using Messenger.Application.DataTransferObjects.Chats;
using Messenger.Application.DataTransferObjects.ChatUsers;

namespace Messenger.Application.Services.ChatUsers
{
    public interface IChatUserService
    {
        Task<ChatInviteLinkViewModel> CreateChatInviteLinkAsync(long chatId);
        Task<ChatDetailsViewModel> JoinChatAsync(string link);
        Task LeaveChatAsync(long chatId);
        Task<ChatDetailsViewModel> BlokChatUserAsync(ChatUserDto chatUserDto);
        Task<ChatDetailsViewModel> UnBlokChatUserAsync(ChatUserDto chatUserDto);
    }
}
