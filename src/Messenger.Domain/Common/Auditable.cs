namespace Messenger.Domain.Common
{
    public abstract class Auditable<TId> : BaseEntity<TId>
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
}
