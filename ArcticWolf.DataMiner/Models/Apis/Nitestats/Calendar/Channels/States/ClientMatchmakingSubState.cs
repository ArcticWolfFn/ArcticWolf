using ArcticWolf.DataMiner.Models.Apis.Nitestats.Calendar.Channels.States.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Models.Apis.Nitestats.Calendar.Channels.States
{
    public class ClientMatchmakingSubState
    {
        [JsonProperty("region")]
        public Dictionary<string, MatchmakingRegion> Region { get; set; }
    }
}
