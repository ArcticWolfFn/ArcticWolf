using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Controllers
{
    [Route("[controller]/api/service/bulk/status")]
    [ApiController]
    public class LightswitchController : ControllerBase
    {
        public ActionResult<List<LightswitchStatus>> GetLightswitchStatus(
          [FromQuery] string serviceId)
        {
            return (ActionResult<List<LightswitchStatus>>)new List<LightswitchStatus>()
      {
        new LightswitchStatus(serviceId.ToLower())
      };
        }
    }
}
