using System.Collections.Generic;
using SignalR.Socket.Server.Abstraction;

namespace SignalR.Socket.Server.ViewModel
{
    public class RetornoUsuariosViewModel
    {
        public IEnumerable<Users> Users { get; set; }
        public string Environment { get; set; }
        public int Total { get; set; }
    }
}
