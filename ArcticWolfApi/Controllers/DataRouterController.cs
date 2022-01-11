using Microsoft.AspNetCore.Mvc;

namespace ArcticWolfApi.Controllers
{
    [Route("[controller]/api/v1")]
    [ApiController]
    public class DataRouterController : ControllerBase
    {
        [HttpPost("public/data")]
        public ActionResult PostDatarouter() => this.NoContent();
    }
}
