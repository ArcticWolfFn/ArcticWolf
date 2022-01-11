using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ArcticWolfApi.Models.Profile
{
    public class AthenaProfileStats
    {
        [JsonProperty("lifetime_wins")]
        public int LifetimeWins;

        [JsonProperty("past_seasons")]
        public object PastSeasons => new();

        [JsonProperty("season")]
        public object Season => new
        {
            numWins = 0,
            numLowBracket = 0,
            numHighBracket = 0
        };

        [JsonProperty("season_match_boost")]
        public int SeasonMatchBoost => 0;

        [JsonProperty("season_friend_match_boost")]
        public int SeasonFriendMatchBoost => 0;

        [JsonProperty("mfa_reward_claimed")]
        public bool MfaRewardClaimed => true;

        [JsonProperty("quest_manager")]
        public object QuestManager => new
        {
            dailyLoginInterval = DateTime.UtcNow.AddDays(1.0),
            dailyQuestRerolls = 1
        };

        [JsonProperty("party_assist_quest")]
        public string PartyAssistQuest => "";

        [JsonProperty("season_num")]
        public int SeasonNum { get; set; }

        [JsonProperty("season_update")]
        public int SeasonUpdate => 0;

        [JsonProperty("book_level")]
        public int BookLevel { get; set; } = 1;

        [JsonProperty("book_xp")]
        public int BookXp => 0;

        [JsonProperty("book_purchased")]
        public bool BookPurchased { get; set; }

        [JsonProperty("purchased_battle_pass_tier_offers")]
        public object PurchasedBattlePassTierOffers => new
        {
        };

        [JsonProperty("permissions")]
        public string[] Permissions => Array.Empty<string>();

        [JsonProperty("vote_data")]
        public object VoteData => new { };

        [JsonProperty("accountLevel")]
        public int AccountLevel { get; set; } = 1;

        [JsonProperty("level")]
        public int Level { get; set; } = 1;

        [JsonProperty("xp")]
        public int Xp => 0;

        [JsonProperty("xp_overflow")]
        public int XpOverflow => 0;

        [JsonProperty("rested_xp")]
        public int RestedXp => 0;

        [JsonProperty("rested_xp_overflow")]
        public int RestedXpOverflow => 0;

        [JsonProperty("rested_xp_mult")]
        public double RestedXpMult => 1.0;

        [JsonProperty("rested_xp_exchange")]
        public double RestedXpExchange => 1.0;

        [JsonProperty("season_first_tracking_bits")]
        public string[] SeasonFirstTrackingBits => Array.Empty<string>();

        [JsonProperty("competitiveIdentity")]
        public object CompetitiveIdentity => new { };

        [JsonProperty("inventory_limit_bonus")]
        public int InventoryLimitBonus => 0;

        [JsonProperty("daily_rewards")]
        public object DailyRewards => new { };

        [JsonProperty("loadouts")]
        public List<string> Loadouts => new()
        {
            "CosmeticLocker:cosmeticlocker_athena"
        };

        [JsonProperty("last_applied_loadout")]
        public string LastAppliedLoadout => "CosmeticLocker:cosmeticlocker_athena";

        [JsonProperty("active_loadout_index")]
        public int ActiveLoadoutIndex => -1;

        [JsonProperty("use_random_loadout")]
        public bool UseRandomLoadout => false;

        [JsonProperty("favorite_character")]
        public string FavoriteCharacter { get; set; }

        [JsonProperty("favorite_backpack")]
        public string FavoriteBackpack { get; set; }

        [JsonProperty("favorite_pickaxe")]
        public string FavoritePickaxe { get; set; }

        [JsonProperty("favorite_glider")]
        public string FavoriteGlider { get; set; }

        [JsonProperty("favorite_skydivecontrail")]
        public string FavoriteSkyDiveContrail { get; set; }

        [JsonProperty("favorite_dance")]
        public string[] FavoriteDance { get; set; }

        [JsonProperty("favorite_itemwraps")]
        public string[] FavoriteItemWraps { get; set; }

        [JsonProperty("favorite_loadingscreen")]
        public string FavoriteLoadingScreen { get; set; }

        [JsonProperty("favorite_musicpack")]
        public string FavoriteMusicpack { get; set; }

        public AthenaProfileStats(int seasonNumber)
        {
            SeasonNum = seasonNumber;
            FavoriteCharacter = Program.CosmeticLoadout["character"].ToString();
            FavoriteBackpack = Program.CosmeticLoadout["backpack"].ToString();
            FavoritePickaxe = Program.CosmeticLoadout["pickaxe"].ToString();
            FavoriteGlider = Program.CosmeticLoadout["glider"].ToString();
            FavoriteSkyDiveContrail = Program.CosmeticLoadout["skydivecontrail"].ToString();
            FavoriteDance = (string[])Program.CosmeticLoadout["dance"];
            FavoriteItemWraps = (string[])Program.CosmeticLoadout["itemwrap"];
            FavoriteLoadingScreen = Program.CosmeticLoadout["loadingscreen"].ToString();
            FavoriteMusicpack = Program.CosmeticLoadout["musicpack"].ToString();
        }
    }
}
