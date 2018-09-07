using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace SignalR.Hub.Server.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private readonly IHubContext<NotifyHub> _hubContext;

        public MessagesController(IHubContext<NotifyHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        public IActionResult Post([FromBody]Message message)
        {
            _hubContext.Clients.All.SendAsync(message.Type, message.Client, message.Payload);
            return Accepted();
        }
    }
}