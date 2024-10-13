using Messenger.Domain.Entities;

namespace Messenger.Application.Helpers.UserContetx
{
    public interface IUserContextService
    {
        long GetCurrentUserId();
        Task<User> GetCurrentUserAsync();
    }
}
