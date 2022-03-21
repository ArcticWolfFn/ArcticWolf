using Newtonsoft.Json;

namespace ArcticWolf.Apis.NiteStats.Models.Calendar.Channels.States
{
    public class StandaloneStoreSubState
    {
        [JsonProperty("activePurchaseLimitingEventIds")]
        public List<object> ActivePurchaseLimitingEventIds { get; set; }

        [JsonProperty("storefront")]
        public object Storefront { get; set; }

        [JsonProperty("rmtPromotionConfig")]
        public List<object> RmtPromotionConfig { get; set; }

        [JsonProperty("storeEnd")]
        public DateTime StoreEnd { get; set; }
    }
}
