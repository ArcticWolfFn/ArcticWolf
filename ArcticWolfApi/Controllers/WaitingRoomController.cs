using Microsoft.AspNetCore.Mvc;

namespace ArcticWolfApi.Controllers
{
    [Route("[controller]/api/")]
    [ApiController]
    public class WaitingRoomController : ControllerBase
    {
        [HttpGet("waitingroom")]
        public ActionResult GetWaitingRoom() => this.NoContent();
    }
}
