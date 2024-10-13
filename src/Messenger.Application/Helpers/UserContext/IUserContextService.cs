using Messenger.Domain.Entities;

namespace Messenger.Application.Helpers.UserContext
{
    public interface IUserContextService
    {
        long GetCurrentUserId();
        Task<User> GetCurrentUserAsync();
    }
}
