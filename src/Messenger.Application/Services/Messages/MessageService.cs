using AutoMapper;
using FluentValidation;
using Messenger.Application.Extensions;
using Messenger.Application.Helpers.UserContext;
using Messenger.Application.Models.DataTransferObjects.Messages;
using Messenger.Application.Models.Results;
using Messenger.Application.Validators.Messages;
using Messenger.Domain.Entities;
using Messenger.Domain.Exceptions;
using Messenger.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ValidationException = FluentValidation.ValidationException;

namespace Messenger.Application.Services.Messages
{
    public class MessageService : IMessagesService
    {
        private readonly MessengerDbContext _messengerDbContext;
        private readonly IUserContextService _userContextService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public MessageService(
            MessengerDbContext messengerDbContext,
            IUserContextService userContextService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _messengerDbContext = messengerDbContext;
            _userContextService = userContextService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<MessageViewModel>> CreateMessageAsync(MessageCreationDto messageCreationDto)
        {
            var userId = _userContextService.GetCurrentUserId();

            var validator = new MessageCreationValidator();
            var validationResult = await validator.ValidateAsync(messageCreationDto);
            if (!validationResult.IsValid)
                throw new ValidationException("Model yaratish uchun yaroqsiz", validationResult.Errors);

            var chatUser = await _messengerDbContext.ChatUsers
                .FirstOrDefaultAsync(x => x.ChatId == messageCreationDto.ChatId && x.UserId == userId);

            if (chatUser is null)
                throw new NotFoundException("Chatga qo'shiling!");

            var message = _mapper.Map<Message>(messageCreationDto);
            message.FromId = userId;

            var entityEntry = await _messengerDbContext.AddAsync(message);
            await _messengerDbContext.SaveChangesAsync();

            var messageViewModel = _mapper.Map<MessageViewModel>(entityEntry.Entity);
            return Result<MessageViewModel>.Success(messageViewModel);
        }

        public async Task<Result<MessageViewModel>> DeleteMessageAsync(Guid id)
        {
            var message = await _messengerDbContext.Messages
                .FirstOrDefaultAsync(x => x.Id == id);

            if (message is null)
                throw new NotFoundException("Xabar topilmadi");

            //adminmi
            var userId = _userContextService.GetCurrentUserId();

            var chatUser = await _messengerDbContext.ChatUsers
                .FirstOrDefaultAsync(x => x.ChatId == message.ChatId && x.UserId == userId);

            if (chatUser is null)
                throw new NotFoundException("Chatga qo'shiling!");

            if (chatUser.IsAdmin || message.FromId == userId)
            {
                var entryEntity = _messengerDbContext.Messages.Remove(message);
                await _messengerDbContext.SaveChangesAsync();

                var messageViewModel = _mapper.Map<MessageViewModel>(entryEntity.Entity);
                return Result<MessageViewModel>.Success(messageViewModel);
            }

            throw new ForbiddenException("Xabarni o'chirishga ruxsat yo'q.");
        }

        public async Task<Result<MessageViewModel>> GetMessageByIdAsync(Guid id)
        {
            var message = await _messengerDbContext.Messages
                .FirstOrDefaultAsync(x => x.Id == id);

            if (message is null)
                throw new NotFoundException("Xabar topilmadi");

            var messageViewModel = _mapper.Map<MessageViewModel>(message);
            return Result<MessageViewModel>.Success(messageViewModel);
        }

        public async Task<Result<List<Message>>> GetMessagesAsync(MessagesPaginationSelectionDto messagesPaginationSelectionDto)
        {
            var queryablePagedList = await _messengerDbContext.Messages
                .ToPagedListAsync(
                    httpContext: _httpContextAccessor.HttpContext,
                    pageIndex: messagesPaginationSelectionDto.Index,
                    pageSize: messagesPaginationSelectionDto.Size);

            var messages = await queryablePagedList
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return Result<List<Message>>.Success(messages);
        }

        public async Task<Result<List<MessageViewModel>>> GetMessagesAsync(MessagesPaginationSelectionByChatDto messagesPaginationSelectionByChatDto)
        {
            var cursorPagedList = await _messengerDbContext.Messages
                .Include(x => x.From)
                .Where(x => x.ChatId == messagesPaginationSelectionByChatDto.ChatId)
                .ToCursorPagedListAsync(
                    httpContext: _httpContextAccessor.HttpContext,
                    pageSize: messagesPaginationSelectionByChatDto.PageSize,
                    previousCursor: messagesPaginationSelectionByChatDto.Previous,
                    nextCursor: messagesPaginationSelectionByChatDto.Next);

            var messagesViewModelList = _mapper.Map<List<MessageViewModel>>(cursorPagedList.OrderBy(x => x.CreatedAt));

            return Result<List<MessageViewModel>>.Success(messagesViewModelList);
        }

        public async Task<Result<MessageViewModel>> UpdateMessageAsync(MessageModificationDto messageModificationDto)
        {
            var message = await _messengerDbContext.Messages
                .FirstOrDefaultAsync(x => x.Id == messageModificationDto.Id);

            if (message is null)
                throw new NotFoundException("Xabar topilmadi");

            //messageni ownerimi
            var userId = _userContextService.GetCurrentUserId();
            
            if(message.FromId != userId)
                throw new ForbiddenException("Xabarni o'zgartirishga ruxsat yo'q.");

            //isvalid
            var validator = new MessageModificationDtoValidator();
            var validationResult = await validator.ValidateAsync(messageModificationDto);
            if (!validationResult.IsValid)
                throw new ValidationException("Model o'zgartirish uchun yaroqsiz", validationResult.Errors);

            message = _mapper.Map(messageModificationDto, message);
            var entryEntity = _messengerDbContext.Messages.Update(message);
            await _messengerDbContext.SaveChangesAsync();

            var messageViewModel = _mapper.Map<MessageViewModel>(entryEntity.Entity);
            return Result<MessageViewModel>.Success(messageViewModel);
        }
    }
}
