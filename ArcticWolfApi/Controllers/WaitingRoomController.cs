using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ArcticWolfApi.Controllers
{
    [Route("[controller]/api/waitingroom")]
    [ApiController]
    public class WaitingRoomController : ControllerBase
    {
        public ActionResult GetWaitingRoom() => (ActionResult)this.NoContent();
    }
}
