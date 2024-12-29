using AutoMapper;
using Messenger.Application.Extensions;
using Messenger.Application.Models.DataTransferObjects.Chats;
using Messenger.Application.Models.DataTransferObjects.Users;
using Messenger.Application.Models.Results;
using Messenger.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Messenger.Application.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MessengerDbContext _messengerDbContext;
        private readonly IMapper _mapper;

        public UserService(
            IHttpContextAccessor httpContextAccessor,
            MessengerDbContext messengerDbContext,
            IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _messengerDbContext = messengerDbContext;
            _mapper = mapper;
        }

        public async Task<Result<List<UserViewModel>>> SearchUsersAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ValidationException("Key null yoki bo'sh belgilarda iborat bo'lolmaydi");

            key = $"%{key}%";

            var users = await _messengerDbContext.Users
                .AsNoTracking()
                .Where(x => EF.Functions.ILike(x.FirstName, key)
                         || EF.Functions.ILike(x.LastName, key)
                         || EF.Functions.ILike(x.UserName, key)
                ).ToListAsync();

            var userViewModels = _mapper.Map<List<UserViewModel>>(users);
            return Result<List<UserViewModel>>.Success(userViewModels);
        }

        public async Task<Result<List<UserViewModel>>> GetAllUsers(UsersFilterModel usersFilterModel)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var users = await _messengerDbContext.Users
                .ToPagedListAsync(
                    httpContext,
                    usersFilterModel.Size,
                    usersFilterModel.Index
                );

            var userViewModels = _mapper.Map<List<UserViewModel>>(users);
            return Result<List<UserViewModel>>.Success(userViewModels);
        }

        public async Task<Result<List<UserViewModel>>> GetContacts(UsersFilterModel usersFilterModel)
            => throw new NotImplementedException();
    }
}
