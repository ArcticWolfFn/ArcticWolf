using Newtonsoft.Json;

namespace ArcticWolfApi.Models.Events
{
    public class Payout
    {
        [JsonProperty("rewardType")]
        public string RewardType { get; set; }

        [JsonProperty("rewardMode")]
        public string RewardMode { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}
