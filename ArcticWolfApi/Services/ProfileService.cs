using ArcticWolfApi.Models.Fortnite;
using ArcticWolfApi.Models.Profile;
using System.Collections.Generic;

namespace ArcticWolfApi.Services
{
    public class ProfileService : IProfileService
    {
        public Profile GenerateAthenaProfile(string id, int seasonNumber, string profileId = "athena")
        {
            ++Program.AthenaRvn;

            Profile profile = new()
            {
                _Id = id,
                Id = id,
                Rvn = Program.AthenaRvn,
                ProfileId = profileId,
                Items = new Dictionary<string, object>(),
                Stats = new ProfileStats()
                {
                    Attributes = (object)new AthenaProfileStats(seasonNumber)
                }
            };

            foreach (string whitelistedCosmetic in Program.WhitelistedCosmetics)
            {
                profile.Items.Add(whitelistedCosmetic, (object)new ProfileItem(whitelistedCosmetic));
            }

            profile.Items.Add(
                "CosmeticLocker:cosmeticlocker_athena", 
                new ProfileItem("CosmeticLocker:cosmeticlocker_athena", 
                new AthenaCosmeticLocker()
                {
                    LockerName = "Locker",
                    BannerIcon = "",
                    BannerColor = "",
                    LockerSlotsData = new AthenaLockerSlotsData()
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
                                new AthenaLockerSlot(null, (string[])Program.CosmeticLoadout["dance"])
                            },
                            {
                                "ItemWrap",
                                new AthenaLockerSlot(null, (string[])Program.CosmeticLoadout["itemwrap"])
                            }
                        }
                    }
                }));

            return profile;
        }

        public Profile GenerateCommonCoreProfile(string id, string profileId = "common_core")
        {
            ++Program.CommonCoreRvn;

            return new Profile()
            {
                _Id = id,
                Id = id,
                Rvn = Program.CommonCoreRvn,
                ProfileId = profileId,
                Items = new(),
                Stats = new()
                {
                    Attributes = new CommonCoreProfileStats()
                }
            };
        }

        public Profile GenerateBlankProfile(string id, string profileId)
        {
            ++Program.CommonCoreRvn;

            return new Profile()
            {
                _Id = id,
                Id = id,
                Rvn = Program.CommonCoreRvn,
                ProfileId = profileId,
                Items = new(),
                Stats = new()
                {
                    Attributes = (new { })
                }
            };
        }
    }
}
