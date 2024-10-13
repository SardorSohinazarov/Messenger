using AutoMapper;
using Messenger.Application.DataTransferObjects.Chats;
using Messenger.Application.DataTransferObjects.Filters;
using Messenger.Application.Helpers.UserContetx;
using Messenger.Domain.Entities;
using Messenger.Domain.Exceptions;
using Messenger.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Application.Services.Chats
{
    public class ChatService : IChatService
    {
        private readonly MessengerDbContext _messengerDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;

        public ChatService(
            MessengerDbContext messengerDbContext,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IUserContextService userContextService)
        {
            _messengerDbContext = messengerDbContext;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<ChatDetailsViewModel> CreateChatAsync(ChatCreationDto chatCreationDto)
        {
            var chat = _mapper.Map<Chat>(chatCreationDto);
            chat.Users = new List<ChatUser>();
            chat.Users.Add(new ChatUser() // Bu xolda User o'zi yaratgan chatga admin bo'p qoladi
            {
                UserId = _userContextService.GetCurrentUserId(),
                Chat = chat,
                IsAdmin = true
            });

            var entryEntity = await _messengerDbContext.AddAsync(chat);
            await _messengerDbContext.SaveChangesAsync();

            return _mapper.Map<ChatDetailsViewModel>(entryEntity.Entity);
        }

        public async Task<ChatViewModel> DeleteAsync(long id)
        {
            //adminmi
            var chat = await _messengerDbContext.Chats
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (chat is null)
                throw new NotFoundException("Chat topilmadi");

            var chatUser = chat.Users.FirstOrDefault(x => x.UserId == _userContextService.GetCurrentUserId());

            if (chatUser is null || !chatUser.IsAdmin)
                throw new ValidationException("Siz faqat o'zingiz yaratgan chatlarni o'chira olasiz.");

            var entryEntity = _messengerDbContext.Chats.Remove(chat);
            await _messengerDbContext.SaveChangesAsync();

            return _mapper.Map<ChatViewModel>(chat);
        }

        public async Task<ChatDetailsViewModel> GetChatAsync(long id)
        {
            var chat = await _messengerDbContext.Chats
                .Include(x => x.Users)
                .Include(x => x.InviteLinks)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(chat is null)
                throw new NotFoundException("Chat topilmadi");

            return _mapper.Map<ChatDetailsViewModel>(chat);
        }

        public async Task<List<Chat>> GetChatsAsync() 
            => await _messengerDbContext.Chats.ToListAsync();

        public async Task<List<ChatViewModel>> GetChatsAsync(ChatFilter filter)
        {
            var userId = _userContextService.GetCurrentUserId();

            var chats = await _messengerDbContext.Chats
                .Include(x => x.Users)
                .Where(x => x.Users.Select(x => x.UserId).Contains(userId))
                .Where(x => filter.UserName == null || x.Username == filter.UserName)
                .Where(x => filter.ChatType == null || x.Type == filter.ChatType)
                .ToListAsync();

            return chats.Select(x => _mapper.Map<ChatViewModel>(x)).ToList();
        }

        public async Task<ChatDetailsViewModel> UpdateChatAsync(ChatModificationDto chatModificationDto)
        {
            //adminmi
            var userId = _userContextService.GetCurrentUserId();
            var chatId = chatModificationDto.Id;

            var chatUser = await _messengerDbContext.ChatUsers
                .FirstOrDefaultAsync(x => x.ChatId == chatId && x.UserId == userId);

            if (chatUser == null || !chatUser.IsAdmin)
                throw new NotFoundException("Siz faqat o'zingiz yaratgan chatlarni o'zgartira olasiz.");

            var chat = _mapper.Map<Chat>(chatModificationDto);
            var entryEntity = _messengerDbContext.Chats.Update(chat);
            await _messengerDbContext.SaveChangesAsync();

            return _mapper.Map<ChatDetailsViewModel>(entryEntity.Entity);
        }
    }
}
