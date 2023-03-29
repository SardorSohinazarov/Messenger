using Microsoft.AspNetCore.SignalR.Client;

namespace Messenger.Web.Pages
{
    public partial class Index
    {
        public string MessageText { get; set; }
        public string UserName { get; set; }

        private readonly List<Tuple<string, string>> messages =
            new List<Tuple<string, string>>();

        private HubConnection _hubConnection;

        protected override async Task OnInitializedAsync()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7232/messages")
                .Build();

            _hubConnection.On<string, string>("GetMessage", GetMessage);

            await _hubConnection.StartAsync();
        }

        private void GetMessage(string username, string message)
        {
            messages.Add(new Tuple<string, string>(username, message));
            StateHasChanged();
        }

        private async Task SendMessageToServer()
        {
            if (_hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.InvokeAsync("SendMessagesAsync", UserName, MessageText);
            }
        }
    }
}
