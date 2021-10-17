using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Calendar
{
    public class ChannelEvent
    {
        public ChannelEvent(string eventType) => this.EventType = eventType;

        [JsonProperty("eventType")]
        public string EventType { get; set; }

        [JsonProperty("activeUntil")]
        public DateTime ActiveUntil => DateTime.MaxValue;

        [JsonProperty("activeSince")]
        public DateTime ActiveSince => DateTime.UtcNow;
    }
}
