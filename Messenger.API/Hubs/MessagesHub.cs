using Microsoft.AspNetCore.SignalR;

namespace Messenger.API.Hubs
{
    public class MessagesHub : Hub
    {
        public HubClientService hubClientService;

        public MessagesHub(HubClientService hubClientService)
        {
            this.hubClientService = hubClientService;
        }

        public override Task OnConnectedAsync()
        {
            this.hubClientService.Clients.Add(Context.ConnectionId);
            return Task.CompletedTask;
        }

        public async Task SendMessagesAsync(string username , string message)
        {
            await Clients.All.SendAsync("GetMessage",username, message);
        }
    }
}
