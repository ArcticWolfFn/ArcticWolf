using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Controllers
{
    [Route("/[controller]/api/")]
    [ApiController]
    public class EulaTrackingController : ControllerBase
    {
        [HttpGet("public/agreements/{agreement}/account/{accountId}")]
        public ActionResult GetUserSetting(string agreement, string accountId)
        {
            return (ActionResult)this.NoContent();
        }
    }
}
