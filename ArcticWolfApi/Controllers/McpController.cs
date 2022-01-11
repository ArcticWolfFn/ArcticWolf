using ArcticWolfApi.Exceptions.Cosmetics;
using ArcticWolfApi.Models.Fortnite;
using ArcticWolfApi.Models.Profile;
using ArcticWolfApi.Models.Profile.Changes;
using ArcticWolfApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Controllers
{
    [Route("fortnite/api/game/v2/profile/{accountId}/client")]
    [ApiController]
    public class McpController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly string _profile;
        private readonly List<object> _changes;
        private readonly int _rvn;

        public McpController(IProfileService profileService, IHttpContextAccessor accessor)
        {
            this._profileService = profileService;
            this._profile = accessor.HttpContext.Request.Query["profileId"].ToString() ?? "common_core";
            if (!int.TryParse((string)accessor.HttpContext.Request.Query["rvn"], out this._rvn))
                this._rvn = -1;
            this._changes = new List<object>();
        }

        [HttpPost("QueryProfile")]
        [HttpPost("ClientQuestLogin")]
        [HttpPost("GetMcpTimeForLogin")]
        [HttpPost("RefreshExpeditions")]
        [HttpPost("IncrementNamedCounterStat")]
        public ActionResult<McpResponse> QueryProfile(string accountId)
        {
            string profile = _profile;

            Profile playerProfile = new();

            switch (profile)
            {
                case "common_core":
                case "profile0":
                    playerProfile = _profileService.GenerateCommonCoreProfile(accountId, _profile); ;
                    break;

                case "athena":
                    playerProfile = _profileService.GenerateAthenaProfile(accountId, Request.GetSeasonNumber());
                    break;

                default:
                    _profileService.GenerateBlankProfile(accountId, _profile);
                    break;
            }

            return new McpResponse(playerProfile, _rvn, _profile, _changes);
        }

        [HttpPost("SetHardcoreModifier")]
        public ActionResult<McpResponse> SetHardcoreModifier(string accountId)
        {
            return new McpResponse(
                _profileService.GenerateAthenaProfile(accountId, Request.GetSeasonNumber()), 
                _rvn, _profile, _changes);
        }

        [HttpPost("SetMtxPlatform")]
        public ActionResult<McpResponse> SetMtxPlatform([FromBody] Commands.Currency.SetMtxPlatform body, string accountId)
        {
            _changes.Add(new McpStatModified("current_mtx_platform", body.NewPlatform.ToString()));
            return new McpResponse(_profileService.GenerateCommonCoreProfile(accountId), _rvn, _profile, _changes);
        }

        [HttpPost("EquipBattleRoyaleCustomization")]
        public ActionResult<McpResponse> EquipBattleRoyaleCustomization([FromBody] Commands.Cosmetics.EquipBattleRoyaleCustomization body, string accountId)
        {
            string lower = body.SlotName.ToString().ToLower();

            if (!((IEnumerable<string>)Program.WhitelistedCosmetics).Contains(body.ItemToSlot) && !(body.ItemToSlot == ""))
            {
                throw new CosmeticsDisallowedException();
            }

            if (lower == "dance")
            {
                ((string[])Program.CosmeticLoadout[lower])[body.IndexWithinSlot] = body.ItemToSlot;
            }
            else
            {
                Program.CosmeticLoadout[lower] = body.ItemToSlot;
            }

            _changes.Add(new McpStatModified("favorite_" + lower, Program.CosmeticLoadout[lower]));

            return new McpResponse(_profileService.GenerateAthenaProfile(accountId, Request.GetSeasonNumber()), _rvn, _profile, _changes, true);
        }

        [HttpPost("SetBattleRoyaleBanner")]
        public ActionResult<McpResponse> SetBattleRoyaleBanner([FromBody] Commands.Cosmetics.SetBattleRoyaleBanner body, string accountId)
        {
            throw new CosmeticsDisallowedException();
        }

        [HttpPost("SetCosmeticLockerSlot")]
        public ActionResult<McpResponse> SetCosmeticLockerSlot([FromBody] Commands.Cosmetics.SetCosmeticLockerSlot body, string accountId)
        {
            string lower = body.Category.ToString().ToLower();

            if (!((IEnumerable<string>)Program.WhitelistedCosmetics).Contains(body.ItemToSlot) && !(body.ItemToSlot == ""))
            {
                throw new CosmeticsDisallowedException();
            }

            if (lower == "dance")
            {
                ((string[])Program.CosmeticLoadout[lower])[body.SlotIndex] = body.ItemToSlot;
            }
            else
            {
                Program.CosmeticLoadout[lower] = body.ItemToSlot;
            }

            AthenaLockerSlotsData athenaLockerSlotsData = new()
            {
                Slots = new Dictionary<string, AthenaLockerSlot>()
                {
                    {
                        "Character",
                        new AthenaLockerSlot(null, new string[1]
                        {
                            Program.CosmeticLoadout["character"].ToString()
                        })
                    },
                    {
                        "Backpack",
                        new AthenaLockerSlot(null, new string[1]
                        {
                            Program.CosmeticLoadout["backpack"].ToString()
                        })
                    },
                    {
                        "Pickaxe",
                        new AthenaLockerSlot(null, new string[1]
                        {
                            Program.CosmeticLoadout["pickaxe"].ToString()
                        })
                    },
                    {
                        "Glider",
                        new AthenaLockerSlot(null, new string[1]
                        {
                            Program.CosmeticLoadout["glider"].ToString()
                        })
                    },
                    {
                        "SkyDiveContrail",
                        new AthenaLockerSlot(null, new string[1]
                        {
                          Program.CosmeticLoadout["skydivecontrail"].ToString()
                        })
                    },
                    {
                        "LoadingScreen",
                        new AthenaLockerSlot(null, new string[1]
                        {
                          Program.CosmeticLoadout["loadingscreen"].ToString()
                        })
                    },
                    {
                        "MusicPack",
                        new AthenaLockerSlot(null, new string[1]
                        {
                          Program.CosmeticLoadout["musicpack"].ToString()
                        })
                    },
                    {
                        "Dance",
                        new AthenaLockerSlot(null, (string[]) Program.CosmeticLoadout["dance"])
                    },
                    {
                        "ItemWrap",
                        new AthenaLockerSlot(null, (string[]) Program.CosmeticLoadout["itemwrap"])
                    }
                }
            };

            _changes.Add(new McpItemAttrChanged(body.LockerItem, "locker_slots_data", athenaLockerSlotsData));

            return new McpResponse(_profileService.GenerateAthenaProfile(accountId, Request.GetSeasonNumber()), _rvn, _profile, _changes, true);
        }

        [HttpPost("SetCosmeticLockerBanner")]
        public ActionResult<McpResponse> SetCosmeticLockerBanner([FromBody] Commands.Cosmetics.SetCosmeticLockerBanner body, string accountId)
        {
            throw new CosmeticsDisallowedException();
        }

        [HttpPost("SetItemFavoriteStatusBatch")]
        public ActionResult<McpResponse> SetItemFavoriteStatusBatch([FromBody] Commands.Cosmetics.SetItemFavoriteStatusBatch body, string accountId)
        {
            throw new CosmeticsDisallowedException();
        }
    }
}
