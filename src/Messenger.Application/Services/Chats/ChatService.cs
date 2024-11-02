using AutoMapper;
using FluentValidation;
using Messenger.Application.DataTransferObjects.Chats;
using Messenger.Application.DataTransferObjects.Filters;
using Messenger.Application.DataTransferObjects.Messages;
using Messenger.Application.Helpers.UserContext;
using Messenger.Application.Validators.Chats;
using Messenger.Domain.Entities;
using Messenger.Domain.Enums;
using Messenger.Domain.Exceptions;
using Messenger.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ValidationException = FluentValidation.ValidationException;

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

        public async Task<ChatDetailsViewModel> CreateChannelAsync(ChannelCreationDto channelCreationDto)
        {
            //validatsiya
            var chat = _mapper.Map<Chat>(channelCreationDto);
            chat.Type = EChatType.Channel;

            return await CreateChatAsync(chat);
        }

        public async Task<ChatDetailsViewModel> CreateGroupAsync(GroupCreationDto groupCreationDto)
        {
            //validatsiya
            var chat = _mapper.Map<Chat>(groupCreationDto);
            chat.Type = EChatType.Group;

            return await CreateChatAsync(chat);
        }

        public async Task<ChatDetailsViewModel> GetOrCreatePrivateChatAsync(long userId)
        {
            var currentUserId = _userContextService.GetCurrentUserId();

            var existingChat = await _messengerDbContext.Chats
                .Include(c => c.Users)
                .FirstOrDefaultAsync(c => c.Type == EChatType.Private 
                                       && c.Users.Any(u => u.UserId == currentUserId)
                                       && c.Users.Any(u => u.UserId == userId));

            if (existingChat is not null)
                return _mapper.Map<ChatDetailsViewModel>(existingChat);

            // Yangi chat yaratamiz
            var newChat = new Chat
            {
                Type = EChatType.Private,
                Users = new List<ChatUser>
                {
                    new ChatUser { UserId = currentUserId },
                    new ChatUser { UserId = userId }
                },
            };

            var entryEntity = await _messengerDbContext.AddAsync(newChat);
            await _messengerDbContext.SaveChangesAsync();

            return _mapper.Map<ChatDetailsViewModel>(entryEntity.Entity);
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

        public async Task<List<ChatViewModel>> GetChatsAsync(string key)
        {
            //TODO: global qidiruv uchun
            throw new NotImplementedException();
        }

        public async Task<List<ChatViewModel>> GetUserChatsAsync()
        {
            var userId = _userContextService.GetCurrentUserId();

            var chats = await _messengerDbContext.Chats
                .Include(x => x.Users)
                .Include(x => x.Messages)
                .ThenInclude(x => x.From)
                .Where(x => x.Users.Select(x => x.UserId).Contains(userId))
                .Select(x => new ChatViewModel()
                {
                    Id = x.Id,
                    Username = x.UserName,
                    Type = x.Type,
                    LastMessage = _mapper.Map<MessageViewModel>(x.Messages.OrderByDescending(x => x.CreatedAt).FirstOrDefault()),
                    LastName = x.LastName,
                    Title = x.Title,
                    FirstName = x.FirstName,
                    Photo = x.Photo
                })
                .ToListAsync();

            var privateChatIds = chats
                .Where(x => x.Type == EChatType.Private)
                .Select(x => x.Id)
                .ToList();

            var users = await _messengerDbContext.ChatUsers
                .Include(x => x.User)
                .Where(x => privateChatIds.Contains(x.ChatId))
                .Where(x => x.UserId != userId)
                .ToListAsync();

            chats = chats.Select(x =>
            {
                if (x.Type == EChatType.Private)
                {
                    var user = users.FirstOrDefault(u => u.ChatId == x.Id);
                    x.Title = $"{user.User.FirstName} {user.User.LastName}";
                    x.Username = user.User.UserName;
                    x.FirstName = user.User.FirstName;
                    x.LastName = user.User.LastName;
                }

                return x;
            }).ToList();

            return chats;
        }

        public async Task<List<ChatViewModel>> GetOwnerChatsAsync()
        {
            var userId = _userContextService.GetCurrentUserId();

            var ownerChats = await _messengerDbContext.Chats
                .Where(x => x.CreatedBy == userId
                         && x.Type != EChatType.Private)
                .ToListAsync();

            return _mapper.Map<List<ChatViewModel>>(ownerChats);
        }

        public async Task<List<ChatViewModel>> GetAdminChatsAsync()
        {
            var userId = _userContextService.GetCurrentUserId();

            var adminChatIds = await _messengerDbContext.ChatUsers
                .Where(x => x.UserId == userId
                         && x.Chat.Type != EChatType.Private
                         && x.IsAdmin)
                .Select(x => x.ChatId)
                .ToListAsync();

            var adminChats = await _messengerDbContext.Chats
                .Where(x => adminChatIds.Contains(x.Id))
                .ToListAsync();

            return _mapper.Map<List<ChatViewModel>>(adminChats);
        }

        public async Task<ChatDetailsViewModel> UpdateChatAsync(ChatModificationDto chatModificationDto)
        {
            var validator = new ChatModificationDtoValidator();
            var validationResult = await validator.ValidateAsync(chatModificationDto);
            if (!validationResult.IsValid)
                throw new ValidationException("Model o'zgatirish uchun yaroqsiz", validationResult.Errors);

            //adminmi
            var userId = _userContextService.GetCurrentUserId();
            var chatId = chatModificationDto.Id;

            var chatUser = await _messengerDbContext.ChatUsers
                .FirstOrDefaultAsync(x => x.ChatId == chatId && x.UserId == userId);

            if (chatUser == null || !chatUser.IsAdmin)
                throw new ForbiddenException("Siz faqat o'zingiz yaratgan chatlarni o'zgartira olasiz.");

            var chat = _mapper.Map<Chat>(chatModificationDto);
            var entryEntity = _messengerDbContext.Chats.Update(chat);
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
                throw new ForbiddenException("Siz faqat o'zingiz yaratgan chatlarni o'chira olasiz.");

            var entryEntity = _messengerDbContext.Chats.Remove(chat);
            await _messengerDbContext.SaveChangesAsync();

            return _mapper.Map<ChatViewModel>(chat);
        }

        public async Task<List<Chat>> GetChatsAsync()
            => await _messengerDbContext.Chats.ToListAsync(); // admin panel sifatida

        private async Task<ChatDetailsViewModel> CreateChatAsync(Chat chat)
        {
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
    }
}
