using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalR.Socket.Server.Abstraction
{
    public class SocketHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var context = Context.GetHttpContext();

            lock (UserSocket.UsersSocket)
            {
                UserSocket.UsersSocket.Add(new Users
                {
                    DateTime = DateTime.Now,
                    Application = context?.Request?.Headers["Host"],
                    Environment = context?.Request?.Headers["Origin"],
                    ConnectionId = Context.ConnectionId,
                    UserName = Context.User?.Identity?.Name ?? Context.ConnectionId
                });
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = UserSocket.UsersSocket?.FirstOrDefault(p => p.ConnectionId == Context?.ConnectionId);

            if (user != null)
            {
                lock (UserSocket.UsersSocket)
                {
                    UserSocket.UsersSocket.Remove(user);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        public Task UpdateClient(string userName)
        {
            var clientId = Context.ConnectionId;

            lock (UserSocket.UsersSocket)
            {
                var user = UserSocket.UsersSocket.FirstOrDefault(p => p.ConnectionId == clientId);

                if (user == null) 
                    return Task.CompletedTask;

                var newUser = new Users
                {
                    UserName = userName,
                    Application = user.Application,
                    ConnectionId = user.ConnectionId,
                    Environment = user.Environment,
                    DateTime = user.DateTime
                };

                var removed = UserSocket.UsersSocket.Remove(user);
                if (removed)
                {
                    UserSocket.UsersSocket.Add(newUser);
                }
            }

            return Task.CompletedTask;
        }

        public async Task SendPrivateMessage(string login, string type, string message, string body)
        {
            var connectionId = UserSocket.UsersSocket.Where(x => x.UserName == login);

            foreach (var connection in connectionId)
            {
                await Clients.Client(connection.ConnectionId).SendAsync("ReceiveMessage", login, type, message, body);
            }
        }

        public async Task SendNotification(string mensagem)
        {
            await Clients.All.SendAsync("ReceiveGenericEvent", mensagem, DateTime.Now.Ticks.ToString());
        }
    }
}
