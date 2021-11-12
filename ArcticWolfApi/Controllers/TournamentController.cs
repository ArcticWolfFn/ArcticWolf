using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Controllers
{
    [ApiController]
    public class TournamentController : ControllerBase
    {
        // idk if this endpoint is even used in newer versions. Tournament data can now be found in Content -> tournamentinfo
        [HttpGet("/fortnite/api/game/v2/events/tournamentandhistory/{accountId}/{region}/{clientType}")]
        public object GetUserSetting(string accountId, string region, string clientType)
        {
            Console.WriteLine("Hit tournamentandhistory");
            return (
                eventTournaments: new List<object>(),
                eventWindowHistories: new List<object>()
            );
        }
    }
}
