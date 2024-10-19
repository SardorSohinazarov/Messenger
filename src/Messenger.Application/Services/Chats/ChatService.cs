﻿using AutoMapper;
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

        public async Task<ChatDetailsViewModel> CreateChatAsync(ChatCreationDto chatCreationDto)
        {
            var validator = new ChatCreationDtoValidator();
            var validationResult = await validator.ValidateAsync(chatCreationDto);
            if (!validationResult.IsValid)
                throw new ValidationException("Model yaratish uchun yaroqsiz",validationResult.Errors);

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
                throw new ForbiddenException("Siz faqat o'zingiz yaratgan chatlarni o'chira olasiz.");

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
                .Include(x => x.Messages)
                .Where(x => x.Users.Select(x => x.UserId).Contains(userId))
                .Where(x => filter.UserName == null || x.Username == filter.UserName)
                .Where(x => filter.ChatType == null || x.Type == filter.ChatType)
                .Select(x => new ChatViewModel()
                {
                    Id = x.Id,
                    Username = x.Username,
                    Type = x.Type,
                    LastMessage = _mapper.Map<MessageViewModel>(x.Messages.OrderByDescending(x => x.CreatedAt).FirstOrDefault()),
                    LastName = x.LastName,
                    Title = x.Title,
                    FirstName = x.FirstName,
                    Photo = x.Photo
                })
                .ToListAsync();

            return chats;
        }

        public async Task<ChatDetailsViewModel> GetOrCreatePrivateChat(long userId)
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
    }
}
