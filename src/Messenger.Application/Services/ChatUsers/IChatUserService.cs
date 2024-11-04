using Messenger.Application.Common.Results;
using Messenger.Application.DataTransferObjects.Chats;
using Messenger.Application.DataTransferObjects.ChatUsers;

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
