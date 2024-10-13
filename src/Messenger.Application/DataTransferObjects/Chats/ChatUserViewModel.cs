using Messenger.Domain.Entities;

namespace Messenger.Application.DataTransferObjects.Chats
{
    public class ChatUserViewModel
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public long ChatId { get; set; }

        public bool IsBlocked { get; set; }
        public bool IsAdmin { get; set; }

        public User User { get; set; }
        public Chat Chat { get; set; }
    }
}
