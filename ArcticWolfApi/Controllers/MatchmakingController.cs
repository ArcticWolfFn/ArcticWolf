using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Controllers
{
    [Route("/fortnite/api/game/v2/matchmakingservice/ticket/player/{playerId}")]
    [ApiController]
    public class MatchmakingController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get(string playerId)
        {
            return (ActionResult)this.NoContent();
        }
    }
}
