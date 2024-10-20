using Messenger.Application.DataTransferObjects.Chats;
using Messenger.Domain.Entities;

namespace Messenger.Application.DataTransferObjects.ChatUsers
{
    public class ChatUserViewModel
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public long ChatId { get; set; }

        public bool IsBlocked { get; set; }
        public bool IsAdmin { get; set; }

        public ChatUserViewModel User { get; set; }
        public ChatViewModel Chat { get; set; }
    }
}
