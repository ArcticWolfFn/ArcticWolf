using Newtonsoft.Json;
using ArcticWolf.Apis.NiteStats.Models.Calendar.Channels.States.Models;

namespace ArcticWolf.Apis.NiteStats.Models.Calendar.Channels.States
{
    public class ClientMatchmakingSubState
    {
        [JsonProperty("region")]
        public Dictionary<string, MatchmakingRegion> Region { get; set; }
    }
}
