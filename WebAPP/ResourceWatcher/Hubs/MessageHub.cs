using Microsoft.AspNetCore.SignalR;

namespace ResourceWatcher.Hubs
{
    public class MessageHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}