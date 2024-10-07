using Microsoft.EntityFrameworkCore;
using Messenger.Domain.Entities;

namespace Messenger.Infrastructure.Persistence
{
    public class MessengerDbContext : DbContext
    {
        //user va unga aloqador setlar
        public DbSet<User> Users { get; set; }

        //chat va unga aloqador setlar
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatInviteLink> ChatInviteLinks { get; set; }
        public DbSet<ChatPhoto> ChatPhotos { get; set; }

        //link tables
        public DbSet<ChatUser> ChatUsers { get; set; }

        //constructorlar
        public MessengerDbContext() { }

        public MessengerDbContext(DbContextOptions<MessengerDbContext> options) : base(options) 
            => Database.Migrate(); // Migratsiyani avtomatik ishga tushirish
    }
}
