using Newtonsoft.Json;
using System.Collections.Generic;

namespace ArcticWolfApi.Models.Events
{
    public class PayoutTable
    {
        [JsonProperty("scoreId")]
        public string ScoreId { get; set; }

        [JsonProperty("scoringType")]
        public string ScoringType { get; set; }

        [JsonProperty("ranks")]
        public List<Rank> Ranks { get; set; }
    }
}
