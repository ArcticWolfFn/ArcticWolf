using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
