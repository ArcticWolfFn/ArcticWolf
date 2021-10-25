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
        [HttpPost("public/agreements/{agreement}/account/{accountId}")] // not working
        public ActionResult GetUserSetting(string agreement, string accountId)
        {
            return (ActionResult)this.NoContent();
        }
    }
}
