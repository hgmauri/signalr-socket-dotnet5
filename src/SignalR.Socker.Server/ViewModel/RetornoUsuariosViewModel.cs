using System.Collections.Generic;
using SignalR.Socker.Server.Abstraction;

namespace SignalR.Socker.Server.ViewModel
{
    public class RetornoUsuariosViewModel
    {
        public IEnumerable<Users> Users { get; set; }
        public string Environment { get; set; }
        public int Total { get; set; }
    }
}
