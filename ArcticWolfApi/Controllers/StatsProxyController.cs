using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Controllers
{
    [Route("/{controller}/api/")]
    [ApiController]
    public class StatsProxyController : ControllerBase
    {
        [HttpGet("statsv2/account/{accountId}")]
        public ActionResult<StatsResponse> GetUserSetting(string accountId)
        {
            StatsResponse response = new();
            response.accountId = accountId;
            return response;
        }

        public class StatsResponse
        {
            public int startTime = 0;
            public long endTime = 9223372036854775807;
            public object statst = new();
            public string accountId = "";
        }
    }
}
