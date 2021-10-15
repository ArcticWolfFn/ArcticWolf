using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Controllers
{
    [Route("[controller]/api/v1/public/data")]
    [ApiController]
    public class DataRouterController : ControllerBase
    {
        public ActionResult PostDatarouter() => (ActionResult)this.NoContent();
    }
}
