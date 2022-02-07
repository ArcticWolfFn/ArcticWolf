using Microsoft.AspNetCore.Mvc;

namespace ArcticWolfApi.Controllers
{
    [Route("/[controller]/api/")]
    [ApiController]
    public class EulaTrackingController : ControllerBase
    {
        [HttpGet("public/agreements/{agreement}/account/{accountId}")]
        public ActionResult GetUserSetting(string agreement, string accountId)
        {
            return NoContent();
        }
    }
}
