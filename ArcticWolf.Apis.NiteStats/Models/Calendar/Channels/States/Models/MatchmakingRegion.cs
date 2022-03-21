using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.Apis.NiteStats.Models.Calendar.Channels.States.Models
{
    public class MatchmakingRegion
    {
        [JsonProperty("eventFlagsForcedOff")]
        public List<string> EventFlagsForcedOff { get; set; }
    }
}
