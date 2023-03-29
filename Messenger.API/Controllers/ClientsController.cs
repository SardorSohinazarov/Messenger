using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly HubClientService hubClientService;

        public ClientsController(HubClientService hubClientService)
        {
            this.hubClientService = hubClientService;
        }

        [HttpGet]
        public ActionResult<List<string>> Get()
        {
            return Ok(this.hubClientService.Clients);
        }
    }
}
