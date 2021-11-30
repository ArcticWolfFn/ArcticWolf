using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Models.Apis.Nitestats.Calendar.Channels
{
    public class Event
    {
        [JsonProperty("eventType")]
        public string EventType { get; set; }

        [JsonProperty("activeUntil")]
        public DateTime ActiveUntil { get; set; }

        [JsonProperty("activeSince")]
        public DateTime ActiveSince { get; set; }
    }
}
