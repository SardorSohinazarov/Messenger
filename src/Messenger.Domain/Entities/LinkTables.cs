using Messenger.Domain.Common;

namespace Messenger.Domain.Entities
{
    public class ChatUser : BaseEntity<Guid>, IAuditable, ISoftDeletable
    {
        public long UserId { get; set; }
        public long ChatId { get; set; }

        public bool IsBlocked { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDeleted { get; set; }

        public User User { get; set; }
        public Chat Chat { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public long LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
}
