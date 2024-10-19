using Messenger.Domain.Entities;
using Messenger.Domain.Exceptions;
using Messenger.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Messenger.Application.Helpers.UserContext
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MessengerDbContext _messengerDbContext;

        public UserContextService(
            IHttpContextAccessor httpContextAccessor,
            MessengerDbContext messengerDbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _messengerDbContext = messengerDbContext;
        }

        public async Task<User> GetCurrentUserAsync()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null && long.TryParse(userIdClaim.Value, out var userId))
                return await _messengerDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            throw new NotFoundException("Foydalanuvchi ID topilmadi");
        }

        public long GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null && long.TryParse(userIdClaim.Value, out var userId))
                return userId;

            throw new NotFoundException("Foydalanuvchi ID topilmadi");
        }
    }
}
