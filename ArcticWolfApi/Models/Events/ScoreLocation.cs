using Newtonsoft.Json;

namespace ArcticWolfApi.Models.Events
{
    public class ScoreLocation
    {
        [JsonProperty("scoreMode")]
        public string ScoreMode { get; set; }

        [JsonProperty("leaderboardId")]
        public string LeaderboardId { get; set; }

        [JsonProperty("scoreId")]
        public string ScoreId { get; set; }

        [JsonProperty("useIndividualScores")]
        public bool UseIndividualScores { get; set; }
    }
}
