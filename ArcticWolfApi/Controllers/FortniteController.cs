using ArcticWolfApi.Models.Fortnite;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ArcticWolfApi.Controllers
{
    [Route("[controller]/api/")]
    [ApiController]
    public class FortniteController : ControllerBase
    {
        [HttpGet("versioncheck")]
        [HttpGet("v2/versioncheck/{platform}")]
        public ActionResult<VersionCheck> CheckUpdateStatus(string platform) => new VersionCheck("NO_UPDATE");

        [HttpPost("game/v2/tryPlayOnPlatform/account/{accountId}")]
        public ActionResult CheckIfPlatformPlayAllowed(string accountId)
        {
            this.Response.ContentType = "text/plain";
            return this.Content("true");
        }

        [HttpGet("game/v2/enabled_features")]
        public ActionResult<string[]> GetEnabledFeatures() => Array.Empty<string>();

        [HttpGet("game/v2/world/info")] // StW I guess
        public ActionResult<object> GetWorldInfo() => (ActionResult<object>)new object();

        [HttpGet("game/v2/twitch/{accountId}")]
        public ActionResult<object> GetTwitchInfo(string accountId) => (ActionResult<object>)new object();

        [HttpGet("game/v2/privacy/account/{accountId}")]
        public ActionResult<PrivacySettings> GetPrivacySettings(
          string accountId)
        {
            return new PrivacySettings(accountId);
        }

        // receipts for Epic Games purchases
        [HttpGet("receipts/v1/account/{accountId}/receipts")]
        public ActionResult<string[]> GetReceipts(string accountId) => Array.Empty<string>();

        [HttpGet("matchmaking/session/findPlayer/{accountId}")]
        public ActionResult<string[]> FindMatchmakingSession(string accountId) => Array.Empty<string>();

        [HttpGet("game/v2/br-inventory/account/{accountId}")]
        public ActionResult<BrInventory> GetBrInventory(string accountId)
        {
            return new BrInventory();
        }

        public class BrInventory
        {
            public Stash stash = new();
        }

        public class Stash
        {
            /// <summary>
            /// Gold Bars
            /// </summary>
            public int globalcash = 0;
        }
    }
}
