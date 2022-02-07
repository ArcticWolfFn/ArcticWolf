using Newtonsoft.Json;
using System.Collections.Generic;

namespace ArcticWolfApi.Models.Events
{
    public class ScoringRule
    {
        [JsonProperty("trackedStat")]
        public string TrackedStat { get; set; }

        [JsonProperty("matchRule")]
        public string MatchRule { get; set; }

        [JsonProperty("rewardTiers")]
        public List<RewardTier> RewardTiers { get; set; }
    }
}
