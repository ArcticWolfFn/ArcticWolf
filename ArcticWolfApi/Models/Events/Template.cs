using Newtonsoft.Json;
using System.Collections.Generic;

namespace ArcticWolfApi.Models.Events
{
    public class Template
    {
        [JsonProperty("gameId")]
        public string GameId { get; set; }

        [JsonProperty("eventTemplateId")]
        public string EventTemplateId { get; set; }

        [JsonProperty("playlistId")]
        public string PlaylistId { get; set; }

        [JsonProperty("matchCap")]
        public int MatchCap { get; set; }

        [JsonProperty("liveSessionAttributes")]
        public List<string> LiveSessionAttributes { get; set; }

        [JsonProperty("scoringRules")]
        public List<ScoringRule> ScoringRules { get; set; }

        [JsonProperty("tiebreakerFormula")]
        public TiebreakerFormula TiebreakerFormula { get; set; }

        [JsonProperty("payoutTable")]
        public List<PayoutTable> PayoutTable { get; set; }
    }
}
