using Messenger.Domain.Common;

namespace Messenger.Domain.Entities
{
    public class ChatUser : Auditable<Guid>, ISoftDeletable
    {
        public long UserId { get; set; }
        public long ChatId { get; set; }

        public bool IsBlocked { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDeleted { get; set; }

        public User User { get; set; }
        public Chat Chat { get; set; }
    }
}
