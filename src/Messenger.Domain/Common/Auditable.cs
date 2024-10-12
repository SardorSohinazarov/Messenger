﻿namespace Messenger.Domain.Common
{
    public abstract class Auditable<TId> : BaseEntity<TId>
    {
        public long CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public long LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
}
