using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Controllers
{
    [Route("[controller]/api/v1")]
    [ApiController]
    public class DataRouterController : ControllerBase
    {
        [HttpPost("public/data")]
        public ActionResult PostDatarouter() => (ActionResult)this.NoContent();
    }
}
