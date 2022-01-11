using ArcticWolfApi.Models.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

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

        // GetEventData
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
            public List<Event> events = new()
            {
                new Event()
                {
                    GameId = "Fortnite",
                    EventId = "epicgames_S15_FNCS_Qualifier2_EU",
                    Regions = new() { "EU" },
                    RegionMappings = new() { EU = "EUCOMP" },
                    Platforms = new() { "Windows" },
                    DisplayDataId = "s15_fncs_week2",
                    EventGroup = "Season15FNCS",
                    AnnouncementTime = DateTime.UtcNow.AddHours(-2),
                    BeginTime = DateTime.UtcNow.AddDays(2),
                    EndTime = DateTime.UtcNow.AddDays(2).AddHours(2),
                    Metadata = new()
                    {
                        AccountLockType = "Season",
                        TeamLockType = "Week",
                        RegionLockType = "Season",
                        MinimumAccountLevel = 30,
                    },
                    EventWindows = new()
                    {
                        new EventWindow()
                        {
                            EventWindowId = "S15_FNCS_Qualifier2_EU_Event1",
                            EventTemplateId = "EventTemplate_S15_FNCS_Qualifier2_EU_Round1",
                            CountdownBeginTime = DateTime.UtcNow.AddHours(-1),
                            BeginTime = DateTime.UtcNow.AddDays(2),
                            EndTime = DateTime.UtcNow.AddDays(2).AddHours(2),
                            Round = 0,
                            PayoutDelay = 30,
                            IsTBD = false,
                            CanLiveSpectate = false,
                            ScoreLocations = new()
                            {
                                new ScoreLocation()
                                {
                                    ScoreMode = "window",
                                    LeaderboardId = "Fortnite_EU",
                                    UseIndividualScores = false,
                                }
                            },
                            Visibility = "locked",
                            RequireAnyTokens = new()
                            {
                                "ARENA_S15_Division10"
                            },
                            RequireNoneTokensCaller = new()
                            {
                                "RegionLock_S15_FNCS_BR",
                                "S15_FNCS_EU_AutoQualified"
                            },
                            AdditionalRequirements = new()
                            {
                                "mfa",
                                "eula:s15_fncs_rules"
                            },
                            Metadata = new()
                            {
                                ServerReplays = true,
                                SubgroupId = "Week2",
                                RoundType = "Qualifiers",
                                LiveSpectateAccessToken = "S15FNCS_Observer_EU",
                            }
                        }
                    }
                }
            };
            public List<Template> templates = new()
            {
                new Template()
                {
                    GameId = "Fortnite",
                    EventTemplateId = "EventTemplate_S15_FNCS_Qualifier2_EU_Round1",
                    PlaylistId = "Playlist_ShowdownTournament_Trios",
                    MatchCap = 10,
                    LiveSessionAttributes = new()
                    {
                        "MATCHSTARTTIME"
                    },
                    ScoringRules = new()
                    {
                        new()
                        {
                            TrackedStat = "PLACEMENT_STAT_INDEX",
                            MatchRule = "lte",
                            RewardTiers = new()
                            {
                                new()
                                {
                                    KeyValue = 1,
                                    PointsEarned = 5,
                                    Multiplicative = false,
                                }
                            }
                        },
                        new()
                        {

                        }
                    },
                    TiebreakerFormula = new()
                    {
                        BasePointsBits = 9,
                        Components = new()
                        {
                            new()
                            {
                                TrackedStat = "VICTORY_ROYALE_STAT",
                                Bits = 4,
                                Multiplier = 0,
                                Aggregation = "sum"
                            }
                        }
                    }
                }
            };
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
