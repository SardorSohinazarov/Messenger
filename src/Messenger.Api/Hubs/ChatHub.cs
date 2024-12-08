using AutoMapper;
using Messenger.Application.Models.DataTransferObjects.Messages;
using Messenger.Domain.Entities;
using Messenger.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Messenger.Api.Hubs
{
    //[Authorize]
    public class ChatHub : Hub
    {
        private readonly MessengerDbContext _messengerDbContext;
        private readonly IMapper _mapper;

        public ChatHub(MessengerDbContext messengerDbContext, IMapper mapper)
        {
            _messengerDbContext = messengerDbContext;
            _mapper = mapper;
        }

        public async Task SendMessage(MessageCreationDto messageCreationDto)
        {
            var message = _mapper.Map<Message>(messageCreationDto);
            var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            message.FromId = long.Parse(userId);
            var entityEntry = await _messengerDbContext.Messages.AddAsync(message);
            await _messengerDbContext.SaveChangesAsync();

            await Clients.All.SendAsync("ReceiveMessage", _mapper.Map<MessageViewModel>(entityEntry.Entity));
        }
    }
}
