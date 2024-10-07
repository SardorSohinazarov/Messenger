using Microsoft.EntityFrameworkCore;
using Messenger.Domain.Entities;

namespace Messenger.Infrastructure.Persistence
{
    public class MessengerDbContext : DbContext
    {
        //user va unga aloqador setlar
        public DbSet<User> Users { get; set; }

        //constructorlar
        public MessengerDbContext() { }

        public MessengerDbContext(DbContextOptions<MessengerDbContext> options) : base(options) 
            => Database.Migrate(); // Migratsiyani avtomatik ishga tushirish
    }
}
