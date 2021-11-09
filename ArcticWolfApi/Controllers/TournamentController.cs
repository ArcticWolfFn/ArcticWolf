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
        [HttpGet("/fortnite/api/game/v2/events/tournamentandhistory/{accountId}/{region}/{clientType}")]
        public object GetUserSetting(string accountId, string region, string clientType)
        {
            return (
                eventTournaments: new List<object>(),
                eventWindowHistories: new List<object>()
            );
        }
    }
}
