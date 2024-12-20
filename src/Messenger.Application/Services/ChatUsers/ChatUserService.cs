﻿using AutoMapper;
using FluentValidation;
using Messenger.Application.Helpers.UserContext;
using Messenger.Application.Models.DataTransferObjects.Chats;
using Messenger.Application.Models.DataTransferObjects.ChatUsers;
using Messenger.Application.Models.Results;
using Messenger.Domain.Entities;
using Messenger.Domain.Enums;
using Messenger.Domain.Exceptions;
using Messenger.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Application.Services.ChatUsers
{
    public class ChatUserService : IChatUserService
    {
        private readonly MessengerDbContext _messengerDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;

        public ChatUserService(
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

        public async Task<Result<ChatDetailsViewModel>> JoinChatAsync(long id)
        {
            var userId = _userContextService.GetCurrentUserId();

            var chatUser = await _messengerDbContext.ChatUsers
                .Include(x => x.Chat)
                .FirstOrDefaultAsync(x => x.ChatId == id
                                       && x.UserId == userId);

            if (chatUser is not null)
            {
                if (chatUser.IsBlocked)
                    throw new ForbiddenException("Siz bloklangansiz.");
                else
                {
                    var chatDetailsViewModel1 = _mapper.Map<ChatDetailsViewModel>(chatUser.Chat);
                    return Result<ChatDetailsViewModel>.Success(chatDetailsViewModel1);
                }
            }

            var chat = await _messengerDbContext.Chats
                .FirstOrDefaultAsync(x => x.Id == id);

            if (chat is null)
                throw new NotFoundException("Chat topilmadi.");

            if (chat.Type == EChatType.Private)
                throw new ValidationException("Private chatga qo'shila olmaysiz.");

            chatUser = new ChatUser()
            {
                ChatId = id,
                UserId = userId
            };

            await _messengerDbContext.ChatUsers.AddAsync(chatUser);
            await _messengerDbContext.SaveChangesAsync();

            var chatDetailsViewModel = _mapper.Map<ChatDetailsViewModel>(chat);
            return Result<ChatDetailsViewModel>.Success(chatDetailsViewModel);
        }

        public async Task<Result<ChatDetailsViewModel>> JoinChatAsync(string link)
        {
            var chatLink = await _messengerDbContext.ChatInviteLinks
                .FirstOrDefaultAsync(x => x.InviteLink == link);

            if (chatLink is null)
                throw new ValidationException("Chat link topilmadi.");

            if (chatLink.ExpireDate < DateTime.UtcNow)
                throw new ValidationException("Link yaroqlilik muddati tugagan.");

            var chat = await _messengerDbContext.Chats
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.Id == chatLink.ChatId);

            if (chat is null)
                throw new NotFoundException("Chat topilmadi.");

            var userId = _userContextService.GetCurrentUserId();

            var chatUser = await _messengerDbContext.ChatUsers
                .FirstOrDefaultAsync(x => x.ChatId == chat.Id && x.UserId == userId);

            if (chatUser == null) // yo'q bo'lsa yaratadi
            {
                chatUser = new ChatUser()
                {
                    ChatId = chat.Id,
                    UserId = userId
                };

                chat.Users.Add(chatUser);
                await _messengerDbContext.SaveChangesAsync();
            }

            if (chatUser.IsBlocked)
                throw new ValidationException("Siz bloklangansiz.");

            var chatDetailsViewModel = _mapper.Map<ChatDetailsViewModel>(chat);
            return Result<ChatDetailsViewModel>.Success(chatDetailsViewModel);
        }

        public async Task<Result<ChatDetailsViewModel>> BlokChatUserAsync(ChatUserDto chatUserDto)
        {
            //adminligini tekshirish kerak
            var userId = _userContextService.GetCurrentUserId();

            var chatUserAdmin = await _messengerDbContext.ChatUsers
                .FirstOrDefaultAsync(x => x.ChatId == chatUserDto.ChatId
                                       && x.UserId == userId);

            if (!chatUserAdmin.IsAdmin)
                throw new ValidationException("Siz admin emassiz. Sizda bunga ruxsat yo'q.");

            var chatUser = await _messengerDbContext.ChatUsers
                .Include(x => x.Chat)
                .FirstOrDefaultAsync(x => x.ChatId == chatUserDto.ChatId
                                       && x.UserId == chatUserDto.UserId);

            if (chatUser is null)
                throw new NotFoundException("Foydalanuvchi yoki chat topilmadi.");

            chatUser.IsBlocked = true;
            await _messengerDbContext.SaveChangesAsync();

            var chatDetailsViewModel = _mapper.Map<ChatDetailsViewModel>(chatUser.Chat);
            return Result<ChatDetailsViewModel>.Success(chatDetailsViewModel);
        }

        public async Task<Result> LeaveChatAsync(long chatId)
        {
            var userId = _userContextService.GetCurrentUserId();

            var chatUser = _messengerDbContext.ChatUsers
                .FirstOrDefault(x => x.ChatId == chatId && x.UserId == userId);

            if (chatUser is null)
                throw new NotFoundException("Foydalanuvchi yoki chat topilmadi.");

            _messengerDbContext.ChatUsers.Remove(chatUser);
            await _messengerDbContext.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<ChatDetailsViewModel>> UnBlokChatUserAsync(ChatUserDto chatUserDto)
        {
            //adminligini tekshirish kerak
            var userId = _userContextService.GetCurrentUserId();

            var chatUserAdmin = await _messengerDbContext.ChatUsers
                .FirstOrDefaultAsync(x => x.ChatId == chatUserDto.ChatId
                                       && x.UserId == userId);

            if (!chatUserAdmin.IsAdmin)
                throw new ValidationException("Siz admin emassiz. Sizda bunga ruxsat yo'q.");

            var chatUser = await _messengerDbContext.ChatUsers
                .Include(x => x.Chat)
                .FirstOrDefaultAsync(x => x.ChatId == chatUserDto.ChatId
                                       && x.UserId == chatUserDto.UserId);

            if (chatUser is null)
                throw new NotFoundException("Foydalanuvchi yoki chat topilmadi.");

            chatUser.IsBlocked = false;
            await _messengerDbContext.SaveChangesAsync();

            var chatDetailsViewModel = _mapper.Map<ChatDetailsViewModel>(chatUser.Chat);
            return Result<ChatDetailsViewModel>.Success(chatDetailsViewModel);
        }

        public async Task<Result<ChatInviteLinkViewModel>> CreateChatInviteLinkAsync(long chatId)
        {
            var chat = await _messengerDbContext.Chats
                .FirstOrDefaultAsync(x => x.Id == chatId
                                       && x.Type != EChatType.Private);

            if (chat is null)
                throw new NotFoundException($"Faqat Group yoki Channel uchun chat link yarata olasiz," +
                                            $" chat topilmadi (id:{chatId}).");

            var inviteLink = Guid.NewGuid().ToString("N");

            var chatLink = new ChatInviteLink()
            {
                ChatId = chatId,
                ExpireDate = DateTime.UtcNow.AddDays(1),
                InviteLink = inviteLink,
            };

            var entryEntity = await _messengerDbContext.AddAsync(chatLink);
            await _messengerDbContext.SaveChangesAsync();

            var chatInviteLinkViewModel = _mapper.Map<ChatInviteLinkViewModel>(entryEntity.Entity);
            return Result<ChatInviteLinkViewModel>.Success(chatInviteLinkViewModel);
        }
    }
}
