﻿using Microsoft.EntityFrameworkCore;
using Messenger.Domain.Entities;
using Messenger.Domain.Common;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Reflection;

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

        //message va unga aloqador setlar
        public DbSet<Message> Messages { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        public MessengerDbContext(
            DbContextOptions<MessengerDbContext> options,
            IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            Database.Migrate(); // Migratsiyani avtomatik ishga tushirish
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // soft delete
            var softDeleteEntries = ChangeTracker
                .Entries<ISoftDeletable>()
                .Where(x => x.State == EntityState.Deleted);

            foreach (var entry in softDeleteEntries)
            {
                entry.State = EntityState.Modified;
                entry.Property(nameof(ISoftDeletable.IsDeleted)).CurrentValue = true;
            }

            //qo'shilayotgan entitylarni CreatedBy va CreatedAt propertylariga qiymat belgilaydi
            var newEntries = ChangeTracker
                .Entries<IAuditable>()
                .Where(x => x.State == EntityState.Added);

            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            long? userId = userIdString is not null ? long.Parse(userIdString) : null;

            foreach (var entry in newEntries)
            {
                if(userId is not null)
                    entry.Property(nameof(IAuditable.CreatedBy)).CurrentValue = userId.Value;
                entry.Property(nameof(IAuditable.CreatedAt)).CurrentValue = DateTime.Now.ToUniversalTime();
            }

            //update bo'layotgan entitylarni LastModifiedBy va LastModifiedAt propertylarini yangilaydi
            var updatedEntries = ChangeTracker
                .Entries<IAuditable>()
                .Where(x => x.State == EntityState.Modified);

            foreach (var entry in updatedEntries)
            {
                if(userId is not null)
                    entry.Property(nameof(IAuditable.LastModifiedBy)).CurrentValue = userId.Value;
                entry.Property(nameof(IAuditable.LastModifiedAt)).CurrentValue = DateTime.UtcNow;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
