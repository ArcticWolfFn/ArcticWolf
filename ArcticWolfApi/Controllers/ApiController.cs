using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Controllers
{
    [Route("/api/v1/")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpPost("user/setting")]
        public ActionResult<List<UserSettingItem>> GetUserSetting(string accountId)
        {
            List<UserSettingItem> usersettings = new();

            IEnumerable<KeyValuePair<string, StringValues>> foundSettingKeys = Request.Query.Where(x => x.Key == "settingKey");

            foreach(KeyValuePair<string, StringValues> settingKey in foundSettingKeys)
            {
                switch(settingKey.Value)
                {
                    case "avatar":
                        usersettings.Add(new UserSettingItem(accountId, settingKey.Value, "cid_003_athena_commando_f_default"));
                        break;

                    case "avatarBackground":
                        usersettings.Add(new UserSettingItem(accountId, settingKey.Value, "[\"#B4F2FE\",\"#00ACF2\",\"#005679\"]"));
                        break;

                    case "appInstalled":
                        usersettings.Add(new UserSettingItem(accountId, settingKey.Value, "init"));
                        break;

                    default:
                        break;
                }
            }

            return usersettings;
        }

        public class UserSettingItem
        {
            public string accountId = "";
            public string key = "";
            public string value = "";

            public UserSettingItem(string accountId, string key, string value)
            {
                this.accountId = accountId;
                this.key = key;
                this.value = value;
            }
        }

        [HttpGet("events/{game}/download/{accountId}")]
        public ActionResult<GameEventsResponse> GetGameEventsForAccount(string game, string accountId, string region, string platform, string teamAccountIds)
        {
            GameEventsResponse response = new();
            response.player.accountId = accountId;
            response.player.gameId = game;

            return response;
        }

        public class GameEventsResponse
        {
            public GERAccount player = new();
            public List<object> events = new();
            public List<object> templates = new();
            public List<object> scores = new();
        }

        public class GERAccount
        {
            public string gameId = "";
            public string accountId = "";
            public List<object> tokens = new();
            public object teams = new();
            public List<object> pendingPayouts = new();
            public object pendingPenalties = new();
            public object persistentScores = new();
            public object groupIdentity = new();
        }

        [HttpPost("assets/Fortnite/{version}/{netcl}")]
        public ActionResult<FnAssetsResponse> GetFnAssets(string version, string netcl)
        {
            FnAssetsResponse response = new();

            return response;
        }

        public class FnAssetsResponse
        {
            public FnAsset FortPlaylistAthena = new();
        }

        public class FnAsset
        {
            public FnAssetMeta meta = new();
            public List<object> assets = new();
        }

        public class FnAssetMeta
        {
            public int promotion = 0;
        }

    }
}
