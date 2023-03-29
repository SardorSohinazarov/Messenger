using Microsoft.AspNetCore.SignalR;

namespace Messenger.API.Hubs
{
    public class MessagesHub : Hub
    {
        public async Task SendMessagesAsync(string username , string message)
        {
            await Clients.All.SendAsync("GetMessage",username, message);
        }
    }
}
