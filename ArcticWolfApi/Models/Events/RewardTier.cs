using Newtonsoft.Json;

namespace ArcticWolfApi.Models.Events
{
    public class RewardTier
    {
        [JsonProperty("keyValue")]
        public int KeyValue { get; set; }

        [JsonProperty("pointsEarned")]
        public int PointsEarned { get; set; }

        [JsonProperty("multiplicative")]
        public bool Multiplicative { get; set; }
    }
}
