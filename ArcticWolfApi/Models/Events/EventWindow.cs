using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Events
{
    public class EventWindow
    {
        [JsonProperty("eventWindowId")]
        public string EventWindowId { get; set; }

        [JsonProperty("eventTemplateId")]
        public string EventTemplateId { get; set; }

        [JsonProperty("countdownBeginTime")]
        public DateTime CountdownBeginTime { get; set; }

        [JsonProperty("beginTime")]
        public DateTime BeginTime { get; set; }

        [JsonProperty("endTime")]
        public DateTime EndTime { get; set; }

        [JsonProperty("blackoutPeriods")]
        public List<object> BlackoutPeriods { get; set; }

        [JsonProperty("round")]
        public int Round { get; set; }

        [JsonProperty("payoutDelay")]
        public int PayoutDelay { get; set; }

        [JsonProperty("isTBD")]
        public bool IsTBD { get; set; }

        [JsonProperty("canLiveSpectate")]
        public bool CanLiveSpectate { get; set; }

        [JsonProperty("scoreLocations")]
        public List<ScoreLocation> ScoreLocations { get; set; }

        [JsonProperty("visibility")]
        public string Visibility { get; set; }

        [JsonProperty("requireAllTokens")]
        public List<string> RequireAllTokens { get; set; }

        [JsonProperty("requireAnyTokens")]
        public List<string> RequireAnyTokens { get; set; }

        [JsonProperty("requireNoneTokensCaller")]
        public List<string> RequireNoneTokensCaller { get; set; }

        [JsonProperty("requireAllTokensCaller")]
        public List<object> RequireAllTokensCaller { get; set; }

        [JsonProperty("requireAnyTokensCaller")]
        public List<object> RequireAnyTokensCaller { get; set; }

        [JsonProperty("additionalRequirements")]
        public List<string> AdditionalRequirements { get; set; }

        [JsonProperty("teammateEligibility")]
        public string TeammateEligibility { get; set; }

        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }
    }
}
