using Messenger.Application.Models.DataTransferObjects.Chats;
using Messenger.Application.Models.DataTransferObjects.Users;
using Messenger.Application.Models.Results;

namespace Messenger.Application.Services.Users
{
    public interface IUserService
    {
        Task<Result<List<UserViewModel>>> GetAllUsers(UsersFilterModel usersFilterModel);
        Task<Result<List<UserViewModel>>> SearchUsersAsync(string key);
        Task<Result<List<UserViewModel>>> GetContacts(UsersFilterModel usersFilterModel);
    }
}
