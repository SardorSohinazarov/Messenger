using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.Infrastructure.Persistence.Configurations
{
    public class ChatUserConfiguration : IEntityTypeConfiguration<ChatUser>
    {
        public void Configure(EntityTypeBuilder<ChatUser> builder)
        {
            builder
                .HasIndex(cu => new { cu.UserId, cu.ChatId }) // Birgalikdagi unikal indeks,
                .IsUnique();                                  // chunki user bitta chatga faqat bir marta qo'shilsin

            builder.HasQueryFilter(x => !x.IsDeleted); // Todo: o'chirilgan chat yana ochiladida unique exception beradi
        }
    }
}
