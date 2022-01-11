using ArcticWolfApi.Models.Lightswitch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Controllers
{
    [Route("[controller]/api/")]
    [ApiController]
    public class LightswitchController : ControllerBase
    {
        [HttpGet("service/bulk/status")]
        public ActionResult<List<LightswitchStatus>> GetLightswitchStatus([FromQuery] string serviceId)
        {
            return new List<LightswitchStatus>()
            {
                new LightswitchStatus(serviceId.ToLower())
            };
        }
    }
}
