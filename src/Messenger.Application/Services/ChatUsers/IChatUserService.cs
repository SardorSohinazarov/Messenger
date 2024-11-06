using Messenger.Application.Models.DataTransferObjects.Chats;
using Messenger.Application.Models.DataTransferObjects.ChatUsers;
using Messenger.Application.Models.Results;

namespace Messenger.Application.Services.ChatUsers
{
    public interface IChatUserService
    {
        Task<Result<ChatInviteLinkViewModel>> CreateChatInviteLinkAsync(long chatId);
        Task<Result<ChatDetailsViewModel>> JoinChatAsync(string link);
        Task<Result<ChatDetailsViewModel>> JoinChatAsync(long id);
        Task<Result> LeaveChatAsync(long chatId);
        Task<Result<ChatDetailsViewModel>> BlokChatUserAsync(ChatUserDto chatUserDto);
        Task<Result<ChatDetailsViewModel>> UnBlokChatUserAsync(ChatUserDto chatUserDto);
    }
}
