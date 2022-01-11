using Newtonsoft.Json;
using System;

namespace ArcticWolfApi.Models.Profile
{
    public class CommonCoreProfileStats
    {
        [JsonProperty("survey_data")]
        public object SurveyData => new
        {
            allSurveysMetadata = new { },
            metadata = new { }
        };

        [JsonProperty("personal_offers")]
        public object PersonalOffers => new { };

        [JsonProperty("intro_game_played")]
        public bool IntroGamePlayed => true;

        [JsonProperty("mtx_affiliate")]
        public string MtxAffiliate => "";

        [JsonProperty("mtx_affiliate_set_time")]
        public DateTime MtxAffiliateSetTime => DateTime.UtcNow.AddDays(-1.0);

        [JsonProperty("mtx_purchase_history")]
        public object MtxPurchaseHistory => new
        {
            refundsUsed = 0,
            refundCredits = 99,
            purchases = Array.Empty<string>()
        };

        [JsonProperty("undo_cooldowns")]
        public string[] UndoCooldowns => Array.Empty<string>();

        [JsonProperty("in_app_purchases")]
        public object InAppPurchases => new { };

        [JsonProperty("import_friends_claimed")]
        public object ImportFriendsClaimed => new { };

        [JsonProperty("inventory_limit_bonus")]
        public int InventoryLimitBonus => 0;

        [JsonProperty("current_mtx_platform")]
        public string CurrentMtxPlatform => "EpicPC";

        [JsonProperty("daily_purchases")]
        public object DailyPurchases => new
        {
            lastInterval = DateTime.UtcNow,
            purchaseList = new { }
        };

        [JsonProperty("weekly_purchases")]
        public object WeeklyPurchases => new
        {
            lastInterval = DateTime.UtcNow,
            purchaseList = new { }
        };

        [JsonProperty("monthly_purchases")]
        public object MonthlyPurchases => new
        {
            lastInterval = DateTime.UtcNow,
            purchaseList = new { }
        };

        [JsonProperty("ban_history")]
        public object BanHistory => new { };

        [JsonProperty("undo_timeout")]
        public DateTime UndoTimeout => DateTime.UtcNow.AddDays(-7.0);

        [JsonProperty("permissions")]
        public string[] Permissions => Array.Empty<string>();

        [JsonProperty("mfa_enabled")]
        public bool MfaEnabled => true;

        [JsonProperty("allowed_to_send_gifts")]
        public bool AllowedToSendGifts => false;

        [JsonProperty("allowed_to_receive_gifts")]
        public bool AllowedToReceiveGifts => false;

        [JsonProperty("gift_history")]
        public object GiftHistory => new
        {
            numSent = 0,
            sentTo = new { },
            num_received = 0,
            receivedFrom = new { },
            gifts = Array.Empty<object>()
        };
    }
}
