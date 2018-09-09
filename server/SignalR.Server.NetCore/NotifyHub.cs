using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SignalR.Hub.Server
{
    public static class UserHandler
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
    }

    //[Authorize]
    public class NotifyHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.Group("SignalRUsers").SendAsync("ReceiveMessage", user, message);
            Console.WriteLine($"User: {user} says {message}");
        }

        public Task SendPrivateMessage(string user, string message)
        {
            return Clients.User(user).SendAsync("ReceiveMessage", message);
        }

        public Task SendMessageToCaller(string message)
        {
            return Clients.Caller.SendAsync("ReceiveMessage", message);
        }

        public Task SendMessageToGroups(string message)
        {
            List<string> groups = new List<string>() { "SignalRUsers" };
            return Clients.Groups(groups).SendAsync("ReceiveMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalRUsers");
            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalRUsers");
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }

    }
}