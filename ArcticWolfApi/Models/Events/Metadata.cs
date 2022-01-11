using Newtonsoft.Json;

namespace ArcticWolfApi.Models.Events
{
    public class Metadata
    {
        [JsonProperty("minimumAccountLevel")]
        public int MinimumAccountLevel { get; set; }

        [JsonProperty("pool")]
        public string Pool { get; set; }

        [JsonProperty("machinelock")]
        public bool? Machinelock { get; set; }

        [JsonProperty("AccountLockType")]
        public string AccountLockType { get; set; }

        [JsonProperty("TeamLockType")]
        public string TeamLockType { get; set; }

        [JsonProperty("RegionLockType")]
        public string RegionLockType { get; set; }

        [JsonProperty("specialType")]
        public string SpecialType { get; set; }

        [JsonProperty("RoundType")]
        public string RoundType { get; set; }

        [JsonProperty("ThresholdToAdvanceDivision")]
        public int ThresholdToAdvanceDivision { get; set; }

        [JsonProperty("divisionRank")]
        public int DivisionRank { get; set; }

        [JsonProperty("ServerReplays")]
        public bool? ServerReplays { get; set; }

        [JsonProperty("RecordCustomMatchScores")]
        public bool? RecordCustomMatchScores { get; set; }

        [JsonProperty("ScheduledMatchmakingInitialDelaySeconds")]
        public int? ScheduledMatchmakingInitialDelaySeconds { get; set; }

        [JsonProperty("ScheduledMatchmakingMatchDelaySeconds")]
        public int? ScheduledMatchmakingMatchDelaySeconds { get; set; }

        [JsonProperty("SubgroupId")]
        public string SubgroupId { get; set; }

        [JsonProperty("UseRounds")]
        public bool? UseRounds { get; set; }

        [JsonProperty("liveSpectateAccessToken")]
        public string LiveSpectateAccessToken { get; set; }
    }
}
