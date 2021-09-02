using System;
using System.Collections.Generic;

namespace SignalR.Socket.Server.Abstraction
{
    public static class UserSocket
    {
        public static readonly IList<Users> UsersSocket = new List<Users>();
    }

    public class Users
    {
        public string ConnectionId { get; set; }
        public string UserName { get; set; }
        public DateTime DateTime { get; set; }
        public string Application { get; set; }
        public string Environment { get; set; }
    }
}
