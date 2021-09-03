using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.Socket.Server.Abstraction;
using SignalR.Socket.Server.ViewModel;

namespace SignalR.Socket.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientesController : ControllerBase
    {
        readonly IHubContext<SocketHub> _hubContext;

        public ClientesController(IHubContext<SocketHub> context)
        {
            _hubContext = context;
        }

        [HttpGet]
        public ActionResult<string> GetClients()
        {
            var users = UserSocket.UsersSocket?.ToList();
            var groups = users?
                .OrderBy(p => p.UserName)
                .GroupBy(g => g.Environment)
                .Select(s => new UsersViewModel
                {
                    Total = s.Count(),
                    Environment = s.Key,
                    Users = s.ToArray()
                });

            return Ok(groups);
        }
    }
}
